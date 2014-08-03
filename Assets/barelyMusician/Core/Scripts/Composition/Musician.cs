using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Musician
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

        // Score (list per bar)
        Dictionary<int, List<Note>[]> score;
        MicroGenerator lineGenerator;

        // Instrument
        Instrument instrument;

        const float MIN_ONSET = 0.01f;
        float initialOnset;

        public Musician(Instrument instrument, MicroGenerator microGenerator)
        {
            this.instrument = instrument;
            initialOnset = Mathf.Max(MIN_ONSET, instrument.Attack);

            lineGenerator = microGenerator;

            Reset();

            active = true;
        }

        public void Reset()
        {
            score = new Dictionary<int, List<Note>[]>();

            instrument.StopAllNotes();
        }

        public List<NoteMeta> GenerateBar(char section, int index, int harmonic)
        {
            return lineGenerator.GenerateLine(section, index, harmonic);
        }

        public void AddBeat(List<NoteMeta> line, SequencerState state, Conductor conductor)
        {
            foreach (NoteMeta noteMeta in line)
            {
                if (Mathf.FloorToInt(noteMeta.Offset * state.BeatCount) - state.CurrentBeat == 0)
                {
                    NoteMeta meta = conductor.TransformNote(noteMeta);

                    AddNote(new Note(meta.Index, meta.Loudness), state.CurrentSection * state.BarCount + state.CurrentBar + meta.Offset, meta.Duration, state.BarLength);
                }
            }
        }

        public void AddNote(Note note, float start, float duration, int barLength)
        {
            // Note On
            addNote(note, start, barLength);

            // Note Off
            Note noteOff = new Note(note.Index, 0.0f);
            addNote(noteOff, start + duration, barLength);
        }

        /**
         * Play next pulse
         **/
        public void PlayPulse(int bar, int pulse, TimbreProperties timbre)
        {
            applyTransformation(timbre);
            play(bar, pulse);
        }

        void addNote(Note note, float onset, int barLength)
        {
            List<Note>[] currentBar;

            int pulse = Mathf.RoundToInt(onset * barLength);
            int bar = pulse / barLength;
            pulse %= barLength;

            if (!score.TryGetValue(bar, out currentBar))
                score[bar] = currentBar = new List<Note>[barLength];

            if (currentBar[pulse] == null)
            {
                currentBar[pulse] = new List<Note>();
            }
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

        void play(int bar, int pulse)
        {
            List<Note>[] currentBar;

            if (score.TryGetValue(bar, out currentBar) && currentBar[pulse] != null)
            {
                foreach (Note note in currentBar[pulse])
                {
                    instrument.PlayNote(note);
                }
            }
        }
    }
}