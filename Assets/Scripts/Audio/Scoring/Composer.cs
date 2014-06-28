using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Composer : MonoBehaviour
    {
        public enum MusicalMode
        {
            IONIAN = 0, DORIAN = 1, PHRYGIAN = 2, LYDIAN = 3, MIXOLYDIAN = 4, AEOLIAN = 5, LOCRIAN = 6
        };

        Performer performer;

        public Automaton1D automaton;

        int fundamentalNote;
        
        int[] notes = { 0, 2, 4, 5, 7, 9, 11, 12 };

        void Start()
        {
            automaton = new Automaton1D(81, 90);
            performer = GetComponent<Performer>();

            // up & down test
            for (int i = 0; i < notes.Length; ++i)
            {
                performer.AddNote(new Note(notes[i], 1.0f), 1.0f + i / 4.0f, 0.25f);
                performer.AddNote(new Note(notes[notes.Length - 1 - i], 1.0f), notes.Length / 4.0f + 1.0f + i / 4.0f, 0.25f);

                performer.AddNote(new Note(notes[i], 1.0f), notes.Length / 2.0f + 1.0f + i / 2.0f, 0.5f);
                performer.AddNote(new Note(notes[(i + 2) % notes.Length], 1.0f), notes.Length / 2.0f + 1.0f + i / 2.0f, 0.5f);
                performer.AddNote(new Note(notes[(i + 4) % notes.Length], 1.0f), notes.Length / 2.0f + 1.0f + i / 2.0f, 0.5f);

                performer.AddNote(new Note(notes[notes.Length - 1 - i], 1.0f), notes.Length + 1.0f + i / 2.0f, 0.5f);
                performer.AddNote(new Note(notes[(notes.Length - 1 - i + 4) % notes.Length], 1.0f), notes.Length + 1.0f + i / 2.0f, 0.5f);
                performer.AddNote(new Note(notes[(notes.Length - 1 - i + 8) % notes.Length], 1.0f), notes.Length + 1.0f + i / 2.0f, 0.5f);
            }

            //for (int i = 0; i < 16; ++i)
            //{
            //    performer.AddNote(new Note(notes[Random.Range(0, notes.Length-1)], Random.Range(0.75f, 1.0f)), 1.0f + Random.Range(0.0f, i * 2.0f), Random.Range(0.05f, 0.25f));
            //}

            //performer.AddNote(new Note(notes[0], 1.0f), 1.0f, 0.25f);
            //performer.AddNote(new Note(notes[1], 1.0f), 1.0f, 0.5f);
            //performer.AddNote(new Note(notes[2], 1.0f), 1.0f, 0.5f);
            //performer.AddNote(new Note(notes[2], 1.0f), 3.0f, 0.5f);
            //performer.AddNote(new Note(notes[3], 1.0f), 3.0f, 0.5f);
            //performer.AddNote(new Note(notes[1], 1.0f), 4.0f, 0.5f);
            //performer.AddNote(new Note(notes[Random.Range(0, notes.Length)], 1.0f), 2.0f, 0.5f);
            //performer.AddNote(new Note(notes[Random.Range(0, notes.Length)], 1.0f), 2.0f, 0.5f);

            //performer.AddNote(new Note(notes[2], 1.0f), 4.0f, 0.25f);
            // performer.AddNote(new Note(notes[Random.Range(0, notes.Length)], 1.0f), 4.0f, 0.25f);
            // performer.AddNote(new Note(notes[Random.Range(0, notes.Length)], 1.0f), 4.0f, 0.25f);
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                automaton.Update();
            }
        }

        void OnEnable()
        {
            AudioEventManager.OnNextBar += OnNextBar;
        }

        void OnDisable()
        {
            AudioEventManager.OnNextBar -= OnNextBar;
        }

        void OnNextBar(int bar)
        {
            //scoreGenerator.Update();

            //for (int i = 0; i < MainClock.BarLength / MainClock.BeatLength; ++i)
            //{
            //    int voiceCount = 0;
            //    for (int j = 0; j < 8; ++j)
            //    {
            //        if (j >0 && j < 7 && scoreGenerator.GetCell(i, j).State == 1)
            //        {
            //            if (++voiceCount > 3)
            //                break;
            //            performer.AddNote(new Note(notes[j - 1], 1.0f), bar + (float)i / MainClock.BeatCount, 1.0f / MainClock.BeatCount);
            //        }
            //    }
            //}
        }
    }
}