using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public abstract class MicroGenerator
    {
        protected int[] pattern;
        public int PatternLength
        {
            get { return pattern.Length; }
        }

        protected MicroGenerator(int length)
        {
            pattern = new int[length];
        }

        public abstract int[] GeneratePattern(int harmonic);
    }
}