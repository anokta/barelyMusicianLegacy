using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class NaberMicroGenerator : MicroGenerator
    {
        Automaton1D ca;
        MarkovChain markov;

        public NaberMicroGenerator(SequencerState sequencerState)
            : base(sequencerState)
        {
            ca = new Automaton1D(81, 90);
            markov = new MarkovChain(ModeGenerator.SCALE_LENGTH);
        }

        public override void Restart()
        {
            base.Restart();

            if(markov != null)
                markov.Reset();
        }

        protected override void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            int keyIndex = (section == SectionType.CHORUS) ? harmonic : 0;

            ca.Update();

            for (int i = 0; i < LineLength; ++i)
            {
                if (ca.GetState(i) == 1)
                {
                    line.Add(new NoteMeta(keyIndex + markov.CurrentState, (float)i / LineLength, 1.0f / LineLength, 1.0f));
                    markov.GenerateNextState();
                }
            }
        }
    }
}