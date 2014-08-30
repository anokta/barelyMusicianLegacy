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
    public class ChordMicroGenerator : MicroGenerator
    {
        int[] chord = { 0, 2, 4, 6, 7 };

        public ChordMicroGenerator(Sequencer sequencer)
            : base(sequencer)
        {
        }

        protected override void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            switch (section)
            {
                case SectionType.VERSE:
                case SectionType.PRE_CHORUS:
                    for (int i = 0; i < 2; ++i)
                    {
                        line.Add(new NoteMeta(harmonic + chord[i], 0.0f, 0.75f));
                    }
                    break;

                case SectionType.CHORUS:
                case SectionType.BRIDGE:
                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < LineLength; ++j)
                        {
                            line.Add(new NoteMeta(harmonic + chord[i], (float)j / LineLength, 0.15f, 0.9f));
                        }
                    }
                    break;
            }
        }
    }
}