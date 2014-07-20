using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public abstract class MesoGenerator
    {
        protected int[] harmonicProgression;
        public int ProgressionLength
        {
            get { return harmonicProgression.Length; }
        }

        protected MesoGenerator(int length)
        {
            harmonicProgression = new int[length];    
        }

        public int GetHarmonic(int index)
        {
            return harmonicProgression[index] - 1;
        }

        public abstract void GenerateProgression(char section);
    }
}