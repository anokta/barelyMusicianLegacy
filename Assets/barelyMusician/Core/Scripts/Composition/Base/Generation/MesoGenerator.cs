using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public abstract class MesoGenerator
    {
        SequencerState state;
        protected int ProgressionLength
        {
            get { return state.BarCount; }
        }

        Dictionary<char, int[]> progressions;
       
        protected MesoGenerator(SequencerState sequencerState)
        {
            state = sequencerState;

            progressions = new Dictionary<char, int[]>();
        }

        public int GetHarmonic(char section, int index)
        {
            int[] progression = null;
            if (!progressions.TryGetValue(section, out progression))
            {
                progression = progressions[section] = new int[ProgressionLength];
                for (int i = 0; i < progression.Length; ++i) progression[i] = 1;
                generateProgression(section, ref progression);
            }

            return progression[index] - 1;
        }

        protected abstract void generateProgression(char section, ref int[] progression);
    }
}