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
    public class SimpleOctaveMicroGenerator : MicroGenerator
    {
        public SimpleOctaveMicroGenerator(Sequencer sequencer)
            : base(sequencer)
        {
        }

        protected override void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            for (int i = 0; i < 2 * LineLength; ++i)
            {
                if (i % 2 == 0)
                {
                    line.Add(new NoteMeta(harmonic, 0.5f * i / LineLength, 1.0f / LineLength, i == 0 ? 1.0f : 0.9f));
                }
                else if(i != 1)
                {
                    line.Add(new NoteMeta(harmonic + ModeGenerator.SCALE_LENGTH, 0.5f * i / LineLength, 1.0f / LineLength, 0.6f));
                }
            }
        }
    }
}