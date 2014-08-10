﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Performer
    {
        // Audio output
        public float Output
        {
            get 
            { 
                float output = instrument.ProcessNext();
                return active ? output : 0.0f;
            }
        }

        bool active;
        public bool Mute
        {
            get { return active; }
            set { active = !value; }
        }

        // Score (note list per bar)
        Dictionary<int, List<Note>[]> score;

        MicroGenerator lineGenerator;
        List<NoteMeta> currentBar;

        // Instrument
        Instrument instrument;

        float initialOnset;

        public Performer(Instrument instrument, MicroGenerator microGenerator)
        {
            this.instrument = instrument;
            initialOnset = instrument.Attack;

            lineGenerator = microGenerator;

            active = true;

            Reset();
        }

        public void Reset()
        {
            score = new Dictionary<int, List<Note>[]>();

            instrument.StopAllNotes();

            lineGenerator.Restart();
        }

        public void GenerateBar(SectionType section, int index, int harmonic)
        {
            currentBar = lineGenerator.GetLine(section, index, harmonic);
        }

        public void AddBeat(Sequencer sequencer, Conductor conductor)
        {
            foreach (NoteMeta noteMeta in currentBar)
            {
                if (Mathf.FloorToInt(noteMeta.Offset * sequencer.BeatCount) == sequencer.CurrentBeat)
                {
                    NoteMeta meta = conductor.TransformNote(noteMeta);

                    float start = sequencer.CurrentSection * sequencer.BarCount + sequencer.CurrentBar + meta.Offset;
                    float end = start + meta.Duration;

                    addNote(new Note(meta.Index, meta.Loudness), start, sequencer.BarLength);
                    addNote(new Note(meta.Index, 0.0f), end, sequencer.BarLength);
                }
            }
        }

        public void Play(int bar, int pulse, TimbreProperties timbre)
        {
            applyTransformation(timbre);

            List<Note>[] currentBar;
            if (score.TryGetValue(bar, out currentBar) && currentBar[pulse] != null)
            {
                foreach (Note note in currentBar[pulse])
                {
                    instrument.PlayNote(note);
                }
            }
        }

        void addNote(Note note, float onset, int barLength)
        {
            int pulse = Mathf.RoundToInt(onset * barLength);
            int bar = pulse / barLength;
            pulse %= barLength;

            List<Note>[] currentBar = null;
            if (!score.TryGetValue(bar, out currentBar))
                score[bar] = currentBar = new List<Note>[barLength];
            if (currentBar[pulse] == null)
                currentBar[pulse] = new List<Note>();

            currentBar[pulse].Add(note);
        }

        void applyTransformation(TimbreProperties timbre)
        {
            instrument.Attack = initialOnset * timbre.NoteOnsetMultiplier;

            foreach (AudioEffect effect in instrument.Effects)
            {
                effect.Apply(timbre);
            }
        }
    }
}