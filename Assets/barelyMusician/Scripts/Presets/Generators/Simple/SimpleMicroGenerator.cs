using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class SimpleMicroGenerator : MicroGenerator
    {
        public SimpleMicroGenerator(int length)
            : base(length)
        {
        }

        public override int[] GeneratePattern(int harmonic)
        {
            pattern[0] = harmonic;

            pattern[1] = harmonic;
            pattern[2] = harmonic + 2;
            pattern[3] = harmonic;
            pattern[4] = harmonic + 4;
            pattern[5] = harmonic;
            pattern[6] = harmonic + 7;
            pattern[7] = harmonic + 4;

            return pattern;
        }
    }
}