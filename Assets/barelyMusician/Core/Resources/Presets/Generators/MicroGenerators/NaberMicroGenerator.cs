// ----------------------------------------------------------------------
//   Adaptive music composition engine implementation for interactive systems.
//
//     Copyright 2014 Alper Gungormusler. All rights reserved.
//
// ------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class NaberMicroGenerator : MicroGenerator
    {
        Automaton1D ca;
        MarkovChain markov;

        int offset;

        public NaberMicroGenerator(Sequencer sequencer)
            : base(sequencer)
        {
            ca = new Automaton1D(81, 90);
            offset = 27;
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
            if (section != SectionType.BRIDGE)
            {
                int keyIndex = (section == SectionType.CHORUS) ? harmonic : 0;

                ca.Update();

                for (int i = 0; i < 2 * LineLength; ++i)
                {
                    if (ca.GetState(offset + i) == 1)
                    {
                        line.Add(new NoteMeta(keyIndex + markov.CurrentState, 0.5f * i / LineLength, 1.0f / LineLength, RandomNumber.NextFloat(0.9f, 0.95f)));

                        markov.GenerateNextState();
                    }
                }
            }
        }
    }
}