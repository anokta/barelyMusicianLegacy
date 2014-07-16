using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Composer : MonoBehaviour
    {
        class NoteInfo
        {
            public int index;
            public float start;
            public float duration;
            public float velocity;

            public NoteInfo(int index, float start, float duration, float velocity = 1.0f)
            {
                this.index = index;
                this.start = start;
                this.duration = duration;
                this.velocity = velocity;
            }
        }

        public Performer performer;

        public int keySignutare;
        public int progressionLength;

        Dictionary<char, List<NoteInfo>> scoreSections;

        void Awake()
        {
            scoreSections = new Dictionary<char, List<NoteInfo>>();
        }

        void Start()
        {
            //melodyGenerator = new Automaton1D(81, 90);

            //chordGenerator = new MarkovChain(8, 1);

            //performer = GameObject.FindGameObjectWithTag("Chord").GetComponent<Performer>();
            //melodyPerformer = GameObject.FindGameObjectWithTag("Melody").GetComponent<Performer>();

            MacroGenerator macro = new MacroGenerator();
            MesoGenerator meso = new MesoGenerator(progressionLength);
            MicroGenerator micro = new MicroGenerator(MainClock.BeatCount);
            ModeGenerator mode = new ModeGenerator();

            mode.GenerateScale();
            macro.GenerateSequence();

            for (int i = 0; i < macro.SequenceLength; ++i)
            {
                List<NoteInfo> currentSection;
                char currentSectionName = macro.GetSectionName(i);

                if (!scoreSections.TryGetValue(currentSectionName, out currentSection))
                {
                    scoreSections[currentSectionName] = currentSection = new List<NoteInfo>();

                    meso.GenerateProgression(currentSectionName);

                    for (int j = 0; j < meso.ProgressionLength; ++j)
                    {
                        int[] pattern = micro.GeneratePattern(meso.GetHarmonic(j) - 1);
                     
                        currentSection.Add(new NoteInfo(mode.GetNote(pattern[0])-12, j + 1, 1.0f, 0.5f));
                        for (int k = 0; k < pattern.Length; ++k)
                        {
                                currentSection.Add(new NoteInfo(mode.GetNote(pattern[k]), j + 1 + (float)k / MainClock.BeatCount, 1.0f / MainClock.BeatCount));
                        }
                    }
                }

                currentSection = scoreSections[currentSectionName];
                foreach(NoteInfo noteInfo in scoreSections[currentSectionName])
                {
                    performer.AddNote(new Note(keySignutare + noteInfo.index, noteInfo.velocity), i * progressionLength + noteInfo.start, noteInfo.duration);
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