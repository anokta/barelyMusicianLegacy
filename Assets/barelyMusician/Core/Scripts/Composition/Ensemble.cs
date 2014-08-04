using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Ensemble
    {
        Dictionary<string, Performer> performers;

        MacroGenerator macro;
        MesoGenerator meso;

        Dictionary<char, List<NoteMeta>[,]> sections;
        char currentSection;

        Conductor conductor;

        public Ensemble(MacroGenerator sequenceGenerator, MesoGenerator sectionGenerator, Conductor conductor)
        {
            performers = new Dictionary<string, Performer>();

            macro = sequenceGenerator;
            meso = sectionGenerator;

            currentSection = '.';
            sections = new Dictionary<char, List<NoteMeta>[,]>();

            this.conductor = conductor;
        }

        public void Register(Sequencer sequencer)
        {
            sequencer.AddSectionListener(OnNextSection);
            sequencer.AddBarListener(OnNextBar);
            sequencer.AddBeatListener(OnNextBeat);
            sequencer.AddPulseListener(OnNextPulse);

            // TODO Do something proper here !
            float minutes = 0.25f;
            macro.GenerateSequence(sequencer.MinuteToSections(minutes));
        }

        public void AddPerformer(string name, Performer performer)
        {
            performers.Add(name, performer);
        }

        public void RemovePerfomer(string name)
        {
            performers.Remove(name);
        }

        public void Stop()
        {
            foreach (Performer performer in performers.Values)
            {
                performer.Reset();
            }
        }

        public float GetOutput()
        {
            float output = 0.0f;

            foreach (Performer performer in performers.Values)
            {
                output += performer.Output;
            }

            return output;
        }

        public void MutePerformer(string name)
        {
            Performer performer = null;
            if(performers.TryGetValue(name, out performer))
            {
                performer.Mute = true;
            }
        }

        public void UnmutePerformer(string name)
        {
            Performer performer = null;
            if (performers.TryGetValue(name, out performer))
            {
                performer.Mute = false;
            }
        }

        //public void ChangeInstrument(string name)
        //{
        //}

        void OnNextSection(SequencerState state)
        {
            currentSection = macro.GetSectionName(state.CurrentSection);

            List<NoteMeta>[,] section = null;
            if (currentSection != '.' & !sections.TryGetValue(currentSection, out section))
            {
                meso.GenerateProgression(currentSection);

                sections[currentSection] = new List<NoteMeta>[performers.Keys.Count, state.BarCount];
            }
        }

        void OnNextBar(SequencerState state)
        {
            List<NoteMeta>[,] section = null;
            if (sections.TryGetValue(currentSection, out section))
            {
                int i = 0;
                if (section[i, state.CurrentBar] == null)
                {
                    foreach (Performer performer in performers.Values)
                    {
                        section[i, state.CurrentBar] = performer.GenerateBar(currentSection, state.CurrentBar, meso.GetHarmonic(state.CurrentBar));
                        i++;
                    }
                }
            }
        }

        void OnNextBeat(SequencerState state)
        {
            int i = 0;
            foreach (Performer performer in performers.Values)
            {
                performer.AddBeat(sections[currentSection][i, state.CurrentBar], state, conductor);
                i++;
            }
        }

        void OnNextPulse(SequencerState state)
        {
            int bar = state.CurrentSection * state.BarCount + state.CurrentBar;
            int pulse = state.CurrentPulse;

            foreach (Performer performer in performers.Values)
            {
                performer.PlayPulse(bar * state.BarLength + pulse, conductor.TimbreProperties);
            }
        }
    }
}