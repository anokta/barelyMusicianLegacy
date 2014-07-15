using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class MicroGenerator
    {
        protected int[] pattern;
        public int PatternLength
        {
            get { return pattern.Length; }
        }

        public MicroGenerator(int length)
        {
            pattern = new int[length];
        }

        public void GeneratePattern(int harmonic)
        {
            harmonic += 3;
            pattern[0] = harmonic;
  
            pattern[1] = -1;
            pattern[2] = harmonic + 2;
            pattern[3] = -1;
            pattern[4] = harmonic + 4;
            pattern[5] = -1;
            pattern[6] = harmonic + 5;
            pattern[7] = -1;

            //for (int i = 0; i < pattern.Length; ++i)
            //{
            //    pattern[i] = harmonic + RandomNumber.NextInt(-2, 8);
            //}
        }

        public List<Note>[] GetGeneratedBar()
        {
            List<Note>[] bar = new List<Note>[MainClock.BarLength];
            
            bar[0] = new List<Note>();
            bar[0].Add(new Note(pattern[0]-12, 0.5f));
            bar[bar.Length-2] = new List<Note>();
            bar[bar.Length-2].Add(new Note(pattern[0]-12, 0.0f));

            for (int i = 0; i < pattern.Length; ++i)
            {
                if (pattern[i] >= 0)
                {
                    if (bar[i * MainClock.BeatLength] == null)
                        bar[i * MainClock.BeatLength] = new List<Note>();
                    bar[i * MainClock.BeatLength].Add(new Note(pattern[i], 1.0f));

                    if (bar[(int)((i + 0.5f) * MainClock.BeatLength)] == null)
                        bar[(int)((i + 0.5f) * MainClock.BeatLength)] = new List<Note>();
                    bar[(int)((i + 0.5f) * MainClock.BeatLength)].Add(new Note(pattern[i], 0.0f));
                }
            }

            return bar;
        }
    }
}