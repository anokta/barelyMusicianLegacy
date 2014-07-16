using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Composer : MonoBehaviour
    {

        //public float stress;

        Performer performer;
        Performer melodyPerformer;

        public MarkovChain chordGenerator;
        public Automaton1D melodyGenerator;

        public int fundamentalNote;

        void Start()
        {
            //melodyGenerator = new Automaton1D(81, 90);

            //chordGenerator = new MarkovChain(8, 1);

            //performer = GameObject.FindGameObjectWithTag("Chord").GetComponent<Performer>();
            melodyPerformer = GameObject.FindGameObjectWithTag("Melody").GetComponent<Performer>();

            MacroGenerator macro = new MacroGenerator();
            MesoGenerator meso = new MesoGenerator(4);
            MicroGenerator micro = new MicroGenerator(MainClock.BeatCount);
            ModeGenerator mode = new ModeGenerator();
            
            mode.GenerateScale();
            macro.GenerateSequence();

            for (int i = 0; i < macro.SequenceLength; ++i)
            {
                meso.GenerateProgression(macro.GetSectionName(i));
   
                for (int j = 0; j < meso.ProgressionLength; ++j)
                {
                    micro.GeneratePattern(mode.GetNote(meso.GetHarmonic(j)));
                    melodyPerformer.AddBar(i * meso.ProgressionLength + j + 1, micro.GetGeneratedBar());
                }
            }


            
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
            //int nextKey = chordGenerator.CurrentState;
            
            //performer.AddNote(new Note(fundamentalNote + notes[nextKey], 1.0f), bar, 1.0f);
            //performer.AddNote(new Note(fundamentalNote + notes[(nextKey + 2) % (notes.Length - 1)] + ((nextKey + 2) / (notes.Length - 1)) * 12, 0.95f), bar, 1.0f);
            //performer.AddNote(new Note(fundamentalNote + notes[(nextKey + 4) % (notes.Length - 1)] + ((nextKey + 4) / (notes.Length - 1)) * 12, 0.78f), bar, 1.0f);


            //melodyGenerator.Update();

            //for (int i = 0; i < MainClock.BeatCount; ++i)
            //{
            //    if (melodyGenerator.GetState(i) == 1)
            //    {
            //        int keyIndex = (nextKey + RandomNumber.NextInt(0, 3));
            //        melodyPerformer.AddNote(new Note(fundamentalNote + notes[keyIndex % (notes.Length - 1)] + (keyIndex / (notes.Length - 1)) * 12, Mathf.Max(0.85f, RandomNumber.NextFloat())), bar + (float)i / MainClock.BeatCount, 1.0f / MainClock.BeatCount); 
            //    }
            //}

            //chordGenerator.GenerateNextState();
        }
    }
}