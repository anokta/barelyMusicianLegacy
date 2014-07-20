using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Composer : MonoBehaviour
    {
        public MacroGenerator macro;
        public MesoGenerator meso;
        public MicroGenerator micro;

        public int progressionLength;

        Dictionary<char, List<NoteMeta>> scoreSections;

        void Awake()
        {
            scoreSections = new Dictionary<char, List<NoteMeta>>();
        }

        void Start()
        {
            macro = new SimpleMacroGenerator();
            meso = new SimpleMesoGenerator(progressionLength);
            micro = new CA1DMicroGenerator(MainClock.BeatCount);

            macro.GenerateSequence();

            for (int i = 0; i < macro.SequenceLength; ++i)
            {
                List<NoteMeta> currentSection;
                char currentSectionName = macro.GetSectionName(i);

                if (!scoreSections.TryGetValue(currentSectionName, out currentSection))
                {
                    scoreSections[currentSectionName] = currentSection = new List<NoteMeta>();

                    meso.GenerateProgression(currentSectionName);

                    for (int j = 0; j < meso.ProgressionLength; ++j)
                    {
                        int[] pattern = micro.GeneratePattern(meso.GetHarmonic(j) - 1);

                        currentSection.Add(new NoteMeta(meso.GetHarmonic(j) - 1 - 7, j + 1, 1.0f, 0.5f));
                        for (int k = 0; k < pattern.Length; ++k)
                        {
                            if(pattern[k] > -100)
                                currentSection.Add(new NoteMeta(pattern[k], j + 1 + (float)k / MainClock.BeatCount, 1.0f / MainClock.BeatCount));
                        }
                    }
                }
            }
        }


        //void OnEnable()
        //{
        //    AudioEventManager.OnNextBar += OnNextBar;
        //}

        //void OnDisable()
        //{
        //    AudioEventManager.OnNextBar -= OnNextBar;
        //}

        //void OnNextBar(int bar)
        //{
        //    //int nextKey = chordGenerator.CurrentState;

        //    //performer.AddNote(new Note(fundamentalNote + notes[nextKey], 1.0f), bar, 1.0f);
        //    //performer.AddNote(new Note(fundamentalNote + notes[(nextKey + 2) % (notes.Length - 1)] + ((nextKey + 2) / (notes.Length - 1)) * 12, 0.95f), bar, 1.0f);
        //    //performer.AddNote(new Note(fundamentalNote + notes[(nextKey + 4) % (notes.Length - 1)] + ((nextKey + 4) / (notes.Length - 1)) * 12, 0.78f), bar, 1.0f);


        //    //melodyGenerator.Update();

        //    //for (int i = 0; i < MainClock.BeatCount; ++i)
        //    //{
        //    //    if (melodyGenerator.GetState(i) == 1)
        //    //    {
        //    //        int keyIndex = (nextKey + RandomNumber.NextInt(0, 3));
        //    //        melodyPerformer.AddNote(new Note(fundamentalNote + notes[keyIndex % (notes.Length - 1)] + (keyIndex / (notes.Length - 1)) * 12, Mathf.Max(0.85f, RandomNumber.NextFloat())), bar + (float)i / MainClock.BeatCount, 1.0f / MainClock.BeatCount); 
        //    //    }
        //    //}

        //    //chordGenerator.GenerateNextState();
        //}

        public void GenerateNextBar(Performer performer)
        {
            int currentBar = performer.currentBarIndex;

            if (currentBar % progressionLength == 0)
            {
                int i = currentBar / progressionLength;
                if (i < macro.SequenceLength)
                {
                    char currentSectionName = macro.GetSectionName(i);
                    List<NoteMeta> currentSection = scoreSections[currentSectionName];

                    foreach (NoteMeta meta in scoreSections[currentSectionName])
                    {
                        performer.AddNote(meta);
                    }
                }
            }
        }
    }
}