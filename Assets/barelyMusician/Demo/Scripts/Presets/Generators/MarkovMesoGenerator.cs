using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class MarkovMesoGenerator : MesoGenerator
    {
        MarkovChain markov;

        public MarkovMesoGenerator(SequencerState sequencerState)
            : base(sequencerState)
        {
            markov = new MarkovChain(8, 1);
        }

        protected override void generateProgression(char section, ref int[] progression)
        {
            markov.Reset();

            for (int i = 0; i < progression.Length; ++i)
            {
                progression[i] = markov.CurrentState + 1;
                markov.GenerateNextState();
            }
        }
    }
}