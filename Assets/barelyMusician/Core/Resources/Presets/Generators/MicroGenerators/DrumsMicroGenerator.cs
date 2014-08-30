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
    public class DrumsMicroGenerator : MicroGenerator
    {

        public DrumsMicroGenerator(Sequencer sequencer)
            : base(sequencer)
        {
        }

        protected override void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            if (section != SectionType.INTRO && section != SectionType.OUTRO)
            {
                float offset = 0.0f;
                float duration = 0.5f / LineLength;

                for (int i = 0; i < LineLength * 2; ++i)
                {
                    // kick
                    if (RandomNumber.NextFloat(0.0f, 1.0f) < 0.025f || (RandomNumber.NextFloat(0.0f, 1.0f) < 0.96f && i % 4 == 0))
                        line.Add(new NoteMeta(0, offset, duration, 1.0f));
                    // snare
                    if ((RandomNumber.NextFloat(0.0f, 1.0f) < 0.025f || i % 8 == 4))
                        line.Add(new NoteMeta(7, offset, duration, 1.0f));
                    // hihat closed
                    if (RandomNumber.NextFloat(0.0f, 1.0f) < 0.01f || i % 4 == 2)
                        line.Add(new NoteMeta(14, offset, duration, RandomNumber.NextFloat(0.5f, 1.0f)));
                    // hihat open
                    if (RandomNumber.NextFloat(0.0f, 1.0f) < 0.01f || i % 16 == 15)
                        line.Add(new NoteMeta(21, offset, duration, 1.0f));

                    offset += duration;
                }
            }
        }
    }
}