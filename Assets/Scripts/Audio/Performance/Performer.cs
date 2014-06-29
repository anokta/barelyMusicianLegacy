using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Performer : MonoBehaviour
    {
        // TODO refactor!
        //static System.Random random = new System.Random();
        //public float stress;

        Instrument instrument;

        Dictionary<int, List<Note>[]> score;
        int currentBarIndex;

        void Awake()
        {
            instrument = GetComponent<Instrument>();

            Reset();
        }

        void OnEnable()
        {
            AudioEventManager.OnNextPulse += OnNextPulse;
            AudioEventManager.OnNextBar += OnNextBar;
            AudioEventManager.OnStop += Reset;
        }

        void OnDisable()
        {
            AudioEventManager.OnNextPulse -= OnNextPulse;
            AudioEventManager.OnNextBar -= OnNextBar;
            AudioEventManager.OnStop += Reset;
        }

        void OnNextBar(int bar)
        {
            currentBarIndex++;
        }

        void OnNextPulse(int pulse)
        {
            List<Note>[] currentBar;

            if (score.TryGetValue(MainClock.currentBar, out currentBar) && currentBar[pulse - 1] != null)
            {
                foreach (Note note in currentBar[pulse - 1])
                {
                    instrument.PlayNote(note);
                }
            }
        }

        void Reset()
        {
            score = new Dictionary<int, List<Note>[]>();
        }

        public void AddNote(Note note, float start, float duration)
        {
            // TODO Performance variance 
            //note.Index += ((float)random.NextDouble() - 0.5f) * stress;

            // Note On
            addNote(note, start);

            // Note Off
            Note noteOff = new Note(note.Index, 0.0f);
            addNote(noteOff, start + duration);
        }

        void addNote(Note note, float onset)
        {
            List<Note>[] currentBar;
            int barLength = MainClock.BarLength;

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
