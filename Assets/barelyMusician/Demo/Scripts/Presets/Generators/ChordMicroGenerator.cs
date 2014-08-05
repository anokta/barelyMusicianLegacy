using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class ChordMicroGenerator : MicroGenerator
    {
        int[] chord = { 0, 2, 4, 6, 7 };

        public ChordMicroGenerator(SequencerState sequencerState)
            : base(sequencerState)
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
                        line.Add(new NoteMeta(harmonic + chord[i], 0.0f, 0.15f));
                        line.Add(new NoteMeta(harmonic + chord[i], 0.25f, 0.15f));
                        line.Add(new NoteMeta(harmonic + chord[i], 0.5f, 0.15f));
                        line.Add(new NoteMeta(harmonic + chord[i], 0.75f, 0.15f));
                    }
                    break;
            }
        }
    }
}