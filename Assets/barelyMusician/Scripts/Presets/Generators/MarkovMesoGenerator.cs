using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class MarkovMesoGenerator : MesoGenerator
    {
        MarkovChain markov;

        public MarkovMesoGenerator(int length)
            : base(length)
        {
            markov = new MarkovChain(8, 1);
        }

        public override void GenerateProgression(char section)
        {
            markov.Reset();

            for (int i = 0; i < ProgressionLength; ++i)
            {
                harmonicProgression[i] = markov.CurrentState + 1;
                markov.GenerateNextState();
            }
        }
    }
}