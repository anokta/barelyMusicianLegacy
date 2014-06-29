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

        //public float stress;

        Performer performer;
        Performer melodyPerformer;

        public MarkovChainO1 chordGenerator;
        public Automaton1D melodyGenerator;

        int fundamentalNote;
        
        int[] notes = { 0, 2, 4, 5, 7, 9, 11, 12 };

        void Start()
        {
            fundamentalNote = -9;
            melodyGenerator = new Automaton1D(81, 90);

            chordGenerator = new MarkovChainO1(8, 1);

            performer = GameObject.FindGameObjectWithTag("Chord").GetComponent<Performer>();
            melodyPerformer = GameObject.FindGameObjectWithTag("Melody").GetComponent<Performer>();
            
            //performer.stress = stress;
            //melodyPerformer.stress = stress;
            
            // up & down test
            //for (int i = 0; i < notes.Length; ++i)
            //{
            //    performer.AddNote(new Note(notes[i], 1.0f), 1.0f + i / 4.0f, 0.25f);
            //    performer.AddNote(new Note(notes[notes.Length - 1 - i], 1.0f), notes.Length / 4.0f + 1.0f + i / 4.0f, 0.25f);

            //    performer.AddNote(new Note(notes[i], 1.0f), notes.Length / 2.0f + 1.0f + i / 2.0f, 0.5f);
            //    performer.AddNote(new Note(notes[(i + 2) % notes.Length], 1.0f), notes.Length / 2.0f + 1.0f + i / 2.0f, 0.5f);
            //    performer.AddNote(new Note(notes[(i + 4) % notes.Length], 1.0f), notes.Length / 2.0f + 1.0f + i / 2.0f, 0.5f);

            //    performer.AddNote(new Note(notes[notes.Length - 1 - i], 1.0f), notes.Length + 1.0f + i / 2.0f, 0.5f);
            //    performer.AddNote(new Note(notes[(notes.Length - 1 - i + 4) % notes.Length], 1.0f), notes.Length + 1.0f + i / 2.0f, 0.5f);
            //    performer.AddNote(new Note(notes[(notes.Length - 1 - i + 8) % notes.Length], 1.0f), notes.Length + 1.0f + i / 2.0f, 0.5f);
            //}

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
                melodyGenerator.Update();
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
            int nextKey = chordGenerator.GetNextState();

            performer.AddNote(new Note(fundamentalNote + notes[nextKey], 1.0f), bar, 1.0f);
            performer.AddNote(new Note(fundamentalNote + notes[(nextKey + 2) % notes.Length], 0.8f), bar, 1.0f);
            performer.AddNote(new Note(fundamentalNote + notes[(nextKey + 2) % notes.Length], 0.9f), bar, 1.0f);

            melodyGenerator.Update();

            for (int i = 0; i < MainClock.BeatCount; ++i)
            {
                if (melodyGenerator.GetState(i) == 1)
                {
                    melodyPerformer.AddNote(new Note(fundamentalNote + notes[(nextKey + RandomNumber.NextInt(-2, 3) + notes.Length) % notes.Length], Mathf.Max(0.85f, RandomNumber.NextFloat())), bar + (float)i / MainClock.BeatCount, 1.0f / MainClock.BeatCount); 
                }
            }
        }
    }
}