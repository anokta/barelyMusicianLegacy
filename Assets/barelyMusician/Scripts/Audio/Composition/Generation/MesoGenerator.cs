using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class MesoGenerator
    {
        protected int[] harmonicProgression;
        public int ProgressionLength
        {
            get { return harmonicProgression.Length; }
        }

        public MesoGenerator(int length)
        {
            harmonicProgression = new int[length];    
        }

        public void GenerateProgression(char section)
        {
            if (section == 'A')
            {
                harmonicProgression[0] = 1;
                harmonicProgression[1] = 4;
                harmonicProgression[2] = 2;
                harmonicProgression[3] = 5;
            }
            else
            {
                harmonicProgression[0] = 1;
                harmonicProgression[1] = 4;
                harmonicProgression[2] = 5;
                harmonicProgression[3] = 1;
            }
        }

        public int GetHarmonic(int index)
        {
            return harmonicProgression[index];
        }
    }
}