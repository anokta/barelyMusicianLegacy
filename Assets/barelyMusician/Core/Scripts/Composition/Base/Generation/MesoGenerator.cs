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

        Dictionary<SectionType, int[]> progressions;
       
        protected MesoGenerator(SequencerState sequencerState)
        {
            state = sequencerState;

            Restart();
        }

        public int GetHarmonic(SectionType section, int index)
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

        public virtual void Restart()
        {
            progressions = new Dictionary<SectionType, int[]>();
        }

        protected abstract void generateProgression(SectionType section, ref int[] progression);
    }
}