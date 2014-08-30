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
    public class YesMicroGenerator : MicroGenerator
    {
        Automaton1D ca;
        MarkovChain markov;

        int offset;

        public YesMicroGenerator(Sequencer sequencer)
            : base(sequencer)
        {
            ca = new Automaton1D(81, 90);
            ca.Update();
            offset = 27;
            markov = new MarkovChain(ModeGenerator.SCALE_LENGTH);
        }

        public override void Restart()
        {
            base.Restart();

            if (markov != null)
                markov.Reset();
        }

        protected override void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            if (section != SectionType.BRIDGE)
            {
                int keyIndex = (section != SectionType.INTRO) ? harmonic : 0;

                if (bar <= 4 / LineLength || bar % 4 == 3 || RandomNumber.NextFloat() < 0.1f)
                {
                    ca.Update();

                    for (int i = 0; i < 2 * LineLength; ++i)
                    {
                        if (ca.GetState(offset + i) == 1)
                        {
                            line.Add(new NoteMeta(keyIndex + markov.CurrentState, 0.5f * i / LineLength, 1.0f / LineLength, RandomNumber.NextFloat(i % (LineLength / 2) == 0 ? 0.9f : 0.8f, 0.95f)));

                            markov.GenerateNextState();
                        }
                    }
                }
                else
                {
                    List<NoteMeta> firstBar = GetLine(section, bar % (4/LineLength+1), 0);
                    line.AddRange(firstBar);
                }
            }
        }
    }
}