using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Performer : MonoBehaviour
    {
        public Conductor conductor;

        Instrument instrument;

        Dictionary<int, List<Note>[]> score;

        void Awake()
        {
            instrument = GetComponent<Instrument>();

            Reset();
        }

        void OnEnable()
        {
            AudioEventManager.OnNextPulse += OnNextPulse;
            AudioEventManager.OnStop += Reset;
        }

        void OnDisable()
        {
            AudioEventManager.OnNextPulse -= OnNextPulse;
            AudioEventManager.OnStop += Reset;
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

        public void AddBar(int index, List<Note>[] bar)
        {
            score[index] = bar;
        }

        public void AddNote(NoteMeta meta)
        {
            // Conduct note
            Note note = new Note(conductor.GetNote(meta.Index), meta.Loudness * conductor.loudness);
            float start = MainClock.currentBar + meta.Offset;
            float end = start + meta.Duration * conductor.articulation;
            
            instrument.Attack = conductor.noteOnset;

            // Note On
            addNote(note, start);

            // Note Off
            Note noteOff = new Note(note.Index, 0.0f);
            addNote(noteOff, end);
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
