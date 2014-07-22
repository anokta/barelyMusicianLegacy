using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Performer
    {
        private float initialOnset;
        public float Onset
        {
            get { return initialOnset; }
            set { instrument.Attack = value; }
        }

        public float Output
        {
            get { return instrument.ProcessNext(); }
        }

        Dictionary<int, List<Note>[]> score;
        Instrument instrument;

        public Performer(Instrument performerInstrument)
        {
            instrument = performerInstrument;
            initialOnset = Mathf.Max(Mathf.Epsilon, instrument.Attack);

            Restart();
        }

        public void Restart()
        {
            score = new Dictionary<int, List<Note>[]>();

            instrument.StopAllNotes();
        }

        public void Play(int bar, int pulse)
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

        public void AddNote(Note note, float start, float duration, int barLength)
        {
            // Note On
            addNote(note, start, barLength);

            // Note Off
            Note noteOff = new Note(note.Index, 0.0f);
            addNote(noteOff, start + duration, barLength);
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
    }
}
