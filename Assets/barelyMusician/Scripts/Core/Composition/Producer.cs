using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Producer
    {
        Dictionary<char, Section> sections;

        MacroGenerator macro;
        MesoGenerator meso;
        MicroGenerator micro;

        Conductor conductor;
        Performer performer;


        public Producer(Instrument instrument, MacroGenerator macroGenerator, MesoGenerator mesoGenerator, MicroGenerator microGenerator)
        {
            performer = new Performer(instrument);

            macro = macroGenerator;
            meso = mesoGenerator;
            micro = microGenerator;

            macro.GenerateSequence();

            Reset();
        }

        public void Reset()
        {
            sections = new Dictionary<char, Section>();

            performer.Restart();
        }

        public void Register(Conductor conductor, Sequencer sequencer)
        {
            this.conductor = conductor;

            sequencer.AddSectionListener(OnNextSection);
            sequencer.AddBeatListener(OnNextBeat);
            sequencer.AddPulseListener(OnNextPulse);
        }

        public void Deregister(Sequencer sequencer)
        {
            sequencer.RemoveSectionListener(OnNextSection);
            sequencer.RemoveBeatListener(OnNextBeat);
            sequencer.RemovePulseListener(OnNextPulse);
        }

        public float GetOutput()
        {
            return performer.Output;
        }

        /**
         * Generate next section
         **/
        void OnNextSection(SequencerState state)
        {
            char sectionName = macro.GetSectionName(state.CurrentSection);
            
            Section section = null;
            if (sectionName != '.' && !sections.TryGetValue(sectionName, out section))
            {
                sections[sectionName] = new Section(meso.ProgressionLength);
                meso.GenerateProgression(sectionName);

                for (int i = 0; i < state.BarCount; ++i)
                {
                    sections[sectionName].AddBar(i, micro.GenerateLine(sectionName, meso.GetHarmonic(i)));
                }
            }
        }

        /**
         * Register next beat to performer
         **/
        void OnNextBeat(SequencerState state)
        {
            char sectionName = macro.GetSectionName(state.CurrentSection);

            Section section = null;
            if (sections.TryGetValue(sectionName, out section))
            {
                foreach (NoteMeta noteMeta in section.GetBar(state.CurrentBar))
                {
                    if (Mathf.FloorToInt(noteMeta.Offset * state.BeatCount) - state.CurrentBeat == 0)
                    {
                        performNote(conductor.TransformNote(noteMeta), state);
                    }
                }
            }
        }

        void performNote(NoteMeta meta, SequencerState state)
        {
            float start = state.CurrentSection * state.BarCount + state.CurrentBar + meta.Offset;

            performer.AddNote(new Note(meta.Index, meta.Loudness), start, meta.Duration, state.BarLength);
        }

        /**
         * Play next pulse
         **/
        void OnNextPulse(SequencerState state)
        {
            performer.ApplyTransformation(conductor.Timbre);
            performer.Play(state.CurrentSection * state.BarCount + state.CurrentBar, state.CurrentPulse);
        }
    }
}