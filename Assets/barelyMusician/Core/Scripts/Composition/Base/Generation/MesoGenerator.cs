using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public abstract class MesoGenerator
    {
        public int[] harmonicProgression;
       
        SequencerState state;
        protected int ProgressionLength
        {
            get { return state.BarCount; }
        }

        protected MesoGenerator(SequencerState sequencerState)
        {
            state = sequencerState;

            harmonicProgression = new int[ProgressionLength];
            for (int i = 0; i < harmonicProgression.Length; ++i)
                harmonicProgression[i] = 1;
        }

        public int GetHarmonic(int index)
        {
            return harmonicProgression[index] - 1;
        }

        public abstract void GenerateProgression(char section);
    }
}