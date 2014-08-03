using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Ensemble
    {
        Dictionary<string, Producer> producers;

        MacroGenerator macro;
        MesoGenerator meso;

        Dictionary<char, List<NoteMeta>[,]> sections;
        char currentSection;

        Conductor conductor;

        public Ensemble(MacroGenerator sequenceGenerator, MesoGenerator sectionGenerator, Conductor conductor)
        {
            producers = new Dictionary<string, Producer>();

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

        public void AddProducer(string name, Producer producer)
        {
            producers.Add(name, producer);
        }

        public void RemoveProducer(string name)
        {
            producers.Remove(name);
        }

        public void Stop()
        {
            foreach (Producer producer in producers.Values)
            {
                producer.Reset();
            }
        }

        public float GetNextOutput()
        {
            float output = 0.0f;

            foreach (Producer producer in producers.Values)
            {
                output += producer.GetOutput();
            }

            return output;
        }

        public void MuteProducer(string name)
        {
            Producer producer = null;
            if(producers.TryGetValue(name, out producer))
            {
                producer.Mute(true);
            }
        }

        public void UnmuteProducer(string name)
        {
            Producer producer = null;
            if(producers.TryGetValue(name, out producer))
            {
                producer.Mute(false);
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

                sections[currentSection] = new List<NoteMeta>[producers.Keys.Count, state.BarCount];
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
                    foreach (Producer producer in producers.Values)
                    {
                        section[i, state.CurrentBar] = producer.GenerateBar(currentSection, state.CurrentBar, meso.GetHarmonic(state.CurrentBar));
                        i++;
                    }
                }
            }
        }

        void OnNextBeat(SequencerState state)
        {
            int i = 0;
            foreach (Producer producer in producers.Values)
            {
                producer.AddBeat(sections[currentSection][i, state.CurrentBar], new Beat(state.CurrentSection * state.BarCount + state.CurrentBar, state.CurrentBeat, state.BeatCount, state.BarLength), conductor);
                i++;
            }
        }

        void OnNextPulse(SequencerState state)
        {
            int bar = state.CurrentSection * state.BarCount + state.CurrentBar;
            int pulse = state.CurrentPulse;

            foreach (Producer producer in producers.Values)
            {
                producer.PlayPulse(bar, pulse, conductor.TimbreProperties);
            }
        }
    }
}