using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class MarkovMesoGenerator : MesoGenerator
    {
        MarkovChain markov;

        public MarkovMesoGenerator(Sequencer sequencer)
            : base(sequencer)
        {
            markov = new MarkovChain(8, 1);
        }

        public override void Restart()
        {
            base.Restart();

            if(markov != null)
                markov.Reset();
        }

        protected override void generateProgression(SectionType section, ref int[] progression)
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