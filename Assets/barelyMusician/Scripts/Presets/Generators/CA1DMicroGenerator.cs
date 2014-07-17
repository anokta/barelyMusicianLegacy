using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class CA1DMicroGenerator : MicroGenerator
    {
        Automaton1D ca;

        public CA1DMicroGenerator(int length)
            : base(length)
        {
            ca = new Automaton1D(81, 90);
        }

        public override int[] GeneratePattern(int harmonic)
        {
            int keyIndex = harmonic;

            ca.Update();

            for (int i = 0; i < MainClock.BeatCount; ++i)
            {
                if (ca.GetState(i) == 1)
                {
                    pattern[i] = keyIndex++;
                }
                else
                    pattern[i] = -100;
            }
            
            return pattern;
        }
    }
}