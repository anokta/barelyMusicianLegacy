using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyMusician
{
    public class Performer : MonoBehaviour
    {
        Instrument instrument;
  
        Dictionary<int, List<Note>[]> score;
        int currentBar;

        void Awake()
        {
            instrument = FindObjectOfType<SynthIntstrument>();

            score = new Dictionary<int, List<Note>[]>();
        }

        void OnEnable()
        {
            AudioEventManager.OnNextPulse += OnNextPulse;
            AudioEventManager.OnNextBar += OnNextBar;
        }

        void OnDisable()
        {
            AudioEventManager.OnNextPulse -= OnNextPulse;
            AudioEventManager.OnNextBar -= OnNextBar;
        }

        void OnNextBar(int bar)
        {
            currentBar++;
        }

        void OnNextPulse(int pulse)
        {
            List<Note>[] currentBar;

            if (score.TryGetValue(MainClock.currentBar, out currentBar) && currentBar[pulse-1] != null)
            {
                foreach (Note note in currentBar[pulse - 1])
                {
                    instrument.PlayNote(note);
                }
            }
        }

        // TODO Any optimizations possible?
        public void AddNote(Note note, float start, float duration)
        {
            // Note On
            int bar = (int)start;
            List<Note>[] currentBar;

            if (!score.TryGetValue(bar, out currentBar))
                score[bar] = currentBar = new List<Note>[MainClock.BarLength];

            int startPulse = Mathf.RoundToInt((start - bar) * MainClock.BarLength);

            if (currentBar[startPulse] == null)
            {
                currentBar[startPulse] = new List<Note>();
            }
            currentBar[startPulse].Add(note);
            
            // Note Off
            float endTime = start - bar + duration;
            if (endTime >= 1.0f)
            {
                bar++;
                endTime -= 1.0f; 
                if (!score.TryGetValue(bar, out currentBar))
                    score[bar] = currentBar = new List<Note>[MainClock.BarLength];
            }

            int endPulse = Mathf.RoundToInt(endTime * MainClock.BarLength);

            Note noteOff = new Note(note.Index, 0.0f);
            if (currentBar[endPulse] == null)
            {
                currentBar[endPulse] = new List<Note>();
            }
            currentBar[endPulse].Add(noteOff);
        }
    }
}
