using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI.Musician
{
    public class Performer : MonoBehaviour
    {
        Instrument instrument;
  
        Dictionary<int, List<Note>[]> score;
        int currentBar;

        int nextNote;
        int[] notes = { 36, 38, 40, 41, 43, 45, 47, 48 };

        void Start()
        {
            instrument = FindObjectOfType<SynthIntstrument>();

            score = new Dictionary<int, List<Note>[]>();

            // up & down test
            for (int i = 0; i < notes.Length; ++i)
            {
                AddNote(new Note(notes[i], 1.0f), 1.0f + i / 4.0f, 0.25f);
                AddNote(new Note(notes[notes.Length - 1 - i], 1.0f), notes.Length/4.0f + 1.0f + i / 4.0f, 0.25f);

                AddNote(new Note(notes[i], 1.0f), notes.Length / 2.0f + 1.0f + i / 2.0f, 0.5f);
                AddNote(new Note(notes[(i + 2) % notes.Length], 1.0f), notes.Length / 2.0f + 1.0f + i / 2.0f, 0.5f);
                AddNote(new Note(notes[(i + 4) % notes.Length], 1.0f), notes.Length / 2.0f + 1.0f + i / 2.0f, 0.5f);

                AddNote(new Note(notes[notes.Length - 1 - i], 1.0f), notes.Length + 1.0f + i / 2.0f, 0.5f);
                AddNote(new Note(notes[(notes.Length - 1 - i + 4) % notes.Length], 1.0f), notes.Length + 1.0f + i / 2.0f, 0.5f);
                AddNote(new Note(notes[(notes.Length - 1 - i + 8) % notes.Length], 1.0f), notes.Length + 1.0f + i / 2.0f, 0.5f);
            }

            //AddNote(new Note(notes[0], 1.0f), 1.0f, 0.5f);
            //AddNote(new Note(notes[1], 1.0f), 1.0f, 0.5f);
            //AddNote(new Note(notes[2], 1.0f), 1.0f, 0.5f);
            //AddNote(new Note(notes[2], 1.0f), 3.0f, 0.5f);
            //AddNote(new Note(notes[3], 1.0f), 3.0f, 0.5f);
            //AddNote(new Note(notes[1], 1.0f), 4.0f, 0.5f);
            //AddNote(new Note(notes[Random.Range(0, notes.Length)], 1.0f), 2.0f, 0.5f);
            //AddNote(new Note(notes[Random.Range(0, notes.Length)], 1.0f), 2.0f, 0.5f);

            //AddNote(new Note(notes[2], 1.0f), 4.0f, 0.25f);
           // AddNote(new Note(notes[Random.Range(0, notes.Length)], 1.0f), 4.0f, 0.25f);
           // AddNote(new Note(notes[Random.Range(0, notes.Length)], 1.0f), 4.0f, 0.25f);
        }

        void Update()
        {
            nextNote = Random.Range(0, notes.Length);
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

            //AddNote(new Note(notes[nextNote], 1.0f), currentBar, 0.5f);
        }

        void OnNextPulse(int pulse)
        {
            List<Note>[] currentBar;

            if (score.TryGetValue(MainClock.barCount, out currentBar) && currentBar[pulse-1] != null)
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
