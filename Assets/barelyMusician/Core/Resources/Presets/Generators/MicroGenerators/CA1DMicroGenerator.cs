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
    public class CA1DMicroGenerator : MicroGenerator
    {
        Automaton1D ca;

        public CA1DMicroGenerator(Sequencer sequencer)
            : base(sequencer)
        {
            ca = new Automaton1D(81, 90);
        }

        protected override void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            int keyIndex = (section == SectionType.CHORUS) ? harmonic : 0;

            if (section == SectionType.VERSE || section == SectionType.PRE_CHORUS || section == SectionType.CHORUS || section == SectionType.BRIDGE)
            {
                ca.Update();

                for (int i = 0; i < LineLength; ++i)
                {
                    if (ca.GetState(i) == 1)
                    {
                        line.Add(new NoteMeta(keyIndex++, (float)i / LineLength, 1.0f / LineLength, 1.0f));
                    }
                }
            }
        }
    }
}