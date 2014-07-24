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
            macro = macroGenerator;
            meso = mesoGenerator;
            micro = microGenerator;

            performer = new Performer(instrument);

            macro.GenerateSequence();

            Reset();
        }

        public void Reset()
        {
            sections = new Dictionary<char, Section>();

            performer.Restart();
        }

        public void SetConductor(Conductor conductor)
        {
            this.conductor = conductor;
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

        public float GetOutput()
        {
            return performer.Output;
        }

        void OnNextSection(SequencerState state)
        {
            // Generate next section
            char sectionName = macro.GetSectionName(state.CurrentSection);
            
            Section section = null;
            if (!sections.TryGetValue(sectionName, out section))
            {
                sections[sectionName] = new Section(meso.ProgressionLength);
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
                foreach (NoteMeta noteMeta in section.GetBar(state.CurrentBar))
                {
                    if (Mathf.FloorToInt(noteMeta.Offset * state.BeatCount) - state.CurrentBeat == 0)
                    {
                        performNote(noteMeta, state);
                    }
                }
            }
        }

        void OnNextPulse(SequencerState state)
        {
            // Play next pulse
            performer.Onset *= conductor.NoteOnsetMultiplier;
            performer.Play(state.CurrentSection * state.BarCount + state.CurrentBar, state.CurrentPulse);
        }

        void performNote(NoteMeta meta, SequencerState state)
        {
            Note note = new Note(conductor.GetNote(meta.Index), meta.Loudness * conductor.LoudnessMultiplier);
            float start = state.CurrentSection * state.BarCount + state.CurrentBar + meta.Offset;
            float duration = meta.Duration * conductor.ArticulationMultiplier; // +meta.Duration * conductor.ArticulationMultiplier * RandomNumber.NextNormal(0.0f, conductor.articulationVariance);

            performer.AddNote(note, start, duration, state.BarLength);
        }
    }
}