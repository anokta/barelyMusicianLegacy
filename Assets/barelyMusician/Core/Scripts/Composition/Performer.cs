using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Performer
    {
        Dictionary<int, List<Note>> score;

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
        MicroGenerator lineGenerator;

        // Instrument
        Instrument instrument;

        float initialOnset;

        public Performer(Instrument instrument, MicroGenerator microGenerator)
        {
            this.instrument = instrument;
            initialOnset = instrument.Attack;

            lineGenerator = microGenerator;

            Reset();

            active = true;

        }

        public void Reset()
        {
            score = new Dictionary<int, List<Note>>();

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
                if (Mathf.FloorToInt(noteMeta.Offset * state.BeatCount) == state.CurrentBeat)
                {
                    NoteMeta meta = conductor.TransformNote(noteMeta);

                    AddNote(new Note(meta.Index, meta.Loudness), (int)(state.BarLength * (state.CurrentSection * state.BarCount + state.CurrentBar + meta.Offset)), (int)(state.BarLength * meta.Duration));
                }
            }
        }

        /**
         * Play next pulse
         **/
        public void PlayPulse(int pulse, TimbreProperties timbre)
        {
            applyTransformation(timbre);

            List<Note> currentPulse;

            if (score.TryGetValue(pulse, out currentPulse))
            {
                foreach (Note note in currentPulse)
                {
                    instrument.PlayNote(note);
                }
            }
        }

        void AddNote(Note note, int start, int duration)
        {
            // Note On
            addNote(note, start);

            // Note Off
            Note noteOff = new Note(note.Index, 0.0f);
            addNote(noteOff, start + duration);
        }

        void addNote(Note note, int onset)
        {
            List<Note> currentPulse;

            if (!score.TryGetValue(onset, out currentPulse))
                score[onset] = currentPulse = new List<Note>();

            currentPulse.Add(note);
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