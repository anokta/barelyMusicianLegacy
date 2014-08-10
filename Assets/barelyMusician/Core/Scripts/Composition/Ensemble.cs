﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Ensemble
    {
        Dictionary<string, Performer> performers;

        MacroGenerator macro;
        public int SongDurationInSections
        {
            get { return macro.SequenceLength; }
            set { macro.SequenceLength = value; }
        }

        MesoGenerator meso;

        Conductor conductor;

        public Ensemble(MacroGenerator sequenceGenerator, MesoGenerator sectionGenerator, Conductor conductor)
        {
            performers = new Dictionary<string, Performer>();

            macro = sequenceGenerator;
            meso = sectionGenerator;

            this.conductor = conductor;
        }

        public void Register(Sequencer sequencer)
        {
            sequencer.AddBarListener(OnNextBar);
            sequencer.AddBeatListener(OnNextBeat);
            sequencer.AddPulseListener(OnNextPulse);
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

            macro.Restart();
            meso.Restart();
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
            if (performers.TryGetValue(name, out performer))
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

        void OnNextBar(Sequencer sequencer)
        {
            SectionType currentSection = macro.GetSection(sequencer.CurrentSection);

            if (currentSection != SectionType.END)
            {
                foreach (Performer performer in performers.Values)
                {
                    performer.GenerateBar(currentSection, sequencer.CurrentBar, meso.GetHarmonic(currentSection, sequencer.CurrentBar));
                }
            }
        }

        void OnNextBeat(Sequencer sequencer)
        {
            foreach (Performer performer in performers.Values)
            {
                performer.AddBeat(sequencer, conductor);
            }
        }

        void OnNextPulse(Sequencer sequencer)
        {
            int bar = sequencer.CurrentSection * sequencer.BarCount + sequencer.CurrentBar;

            foreach (Performer performer in performers.Values)
            {
                performer.Play(bar, sequencer.CurrentPulse, conductor.TimbreProperties);
            }
        }
    }
}