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

        public Conductor conductor;
        public Performer performer;

        public Producer(Instrument instrument, MacroGenerator macroGenerator, MesoGenerator mesoGenerator, MicroGenerator microGenerator)
        {
            sections = new Dictionary<char, Section>();

            macro = macroGenerator;
            meso = mesoGenerator;
            micro = microGenerator;

            performer = new Performer(instrument, null);

            macro.GenerateSequence();
        }

        public void RegisterSequencer(Sequencer sequencer)
        {
            sequencer.AddSectionListener(OnNextSection);
            sequencer.AddBeatListener(OnNextBeat);
            sequencer.AddPulseListener(OnNextPulse);
        }

        public void DeregisterSequencer(Sequencer sequencer)
        {
            sequencer.RemoveSectionListener(OnNextSection);
            sequencer.RemoveBeatListener(OnNextBeat);
            sequencer.RemovePulseListener(OnNextPulse);
        }

        void OnNextSection(SequencerState state)
        {
            // Generate next section
            char sectionName = macro.GetSectionName(state.CurrentSection);
            
            Section section = null;
            if (!sections.TryGetValue(sectionName, out section))
            {
                sections[sectionName] = new Section(state.BarCount);

                meso.GenerateProgression(sectionName);
                for (int i = 0; i < state.BarCount; ++i)
                {
                    sections[sectionName].AddBar(i, micro.GenerateLine(sectionName, meso.GetHarmonic(i)));
                }
            }
        }

        void OnNextBeat(SequencerState state)
        {
            // Add next beat to performer
            char sectionName = macro.GetSectionName(state.CurrentSection);

            Section section = null;
            if (sections.TryGetValue(sectionName, out section))
            {
                foreach (NoteMeta meta in section.GetBar(state.CurrentBar))
                {
                    if (Mathf.FloorToInt(meta.Offset * state.BeatCount) - state.CurrentBeat == 0)
                    {
                        Note note = new Note(conductor.GetNote(meta.Index), meta.Loudness * conductor.loudness);
                        float start = state.CurrentSection * state.BarCount + state.CurrentBar + meta.Offset;
                        float duration = meta.Duration * conductor.articulation;

                        performer.AddNote(note, start, duration, state.BarLength);
                    }
                }
            }
        }

        void OnNextPulse(SequencerState state)
        {
            // Play next pulse
            performer.Onset = conductor.noteOnset;
            performer.Play(state.CurrentSection * state.BarCount + state.CurrentBar, state.CurrentPulse);
        }
    }
}