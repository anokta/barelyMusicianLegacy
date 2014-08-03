using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Ensemble
    {
        Dictionary<string, Musician> musicians;

        MacroGenerator macro;
        MesoGenerator meso;

        Dictionary<char, List<NoteMeta>[,]> sections;
        char currentSection;

        Conductor conductor;

        public Ensemble(MacroGenerator sequenceGenerator, MesoGenerator sectionGenerator, Conductor conductor)
        {
            musicians = new Dictionary<string, Musician>();

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

            float minutes = 0.25f;
            macro.GenerateSequence(sequencer.MinuteToSections(minutes));
        }

        public void AddMusician(string name, Musician producer)
        {
            musicians.Add(name, producer);
        }

        public void RemovePerfomer(string name)
        {
            musicians.Remove(name);
        }

        public void Stop()
        {
            foreach (Musician musician in musicians.Values)
            {
                musician.Reset();
            }
        }

        public float GetNextOutput()
        {
            float output = 0.0f;

            foreach (Musician musician in musicians.Values)
            {
                output += musician.Output;
            }

            return output;
        }

        public void MuteMusician(string name)
        {
            Musician musician = null;
            if(musicians.TryGetValue(name, out musician))
            {
                musician.Mute = true;
            }
        }

        public void UnmuteMusician(string name)
        {
            Musician musician = null;
            if (musicians.TryGetValue(name, out musician))
            {
                musician.Mute = false;
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

                sections[currentSection] = new List<NoteMeta>[musicians.Keys.Count, state.BarCount];
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
                    foreach (Musician musician in musicians.Values)
                    {
                        section[i, state.CurrentBar] = musician.GenerateBar(currentSection, state.CurrentBar, meso.GetHarmonic(state.CurrentBar));
                        i++;
                    }
                }
            }
        }

        void OnNextBeat(SequencerState state)
        {
            int i = 0;
            foreach (Musician musician in musicians.Values)
            {
                musician.AddBeat(sections[currentSection][i, state.CurrentBar], state, conductor);
                i++;
            }
        }

        void OnNextPulse(SequencerState state)
        {
            int bar = state.CurrentSection * state.BarCount + state.CurrentBar;
            int pulse = state.CurrentPulse;

            foreach (Musician musician in musicians.Values)
            {
                musician.PlayPulse(bar, pulse, conductor.TimbreProperties);
            }
        }
    }
}