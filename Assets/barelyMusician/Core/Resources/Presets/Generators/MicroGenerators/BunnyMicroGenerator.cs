using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class BunnyMicroGenerator : MicroGenerator
    {
        int lastNote;

        public BunnyMicroGenerator(Sequencer sequencer)
            : base(sequencer)
        {
            lastNote = 0;
        }

        protected override void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            for (int i = 0; i < 2 * LineLength; ++i)
            {
                if (i % 2 == 0) // || RandomNumber.NextFloat() < 0.01f)
                {
                    lastNote += RandomNumber.NextInt(-ModeGenerator.SCALE_LENGTH / 4, ModeGenerator.SCALE_LENGTH / 2);
                    lastNote = System.Math.Max(-ModeGenerator.SCALE_LENGTH / 2, System.Math.Min(ModeGenerator.SCALE_LENGTH, lastNote));

                    line.Add(new NoteMeta(harmonic + lastNote, (float)i / LineLength / 2.0f, 1.0f / LineLength, RandomNumber.NextFloat(0.80f, 0.85f)));
                }
            }
        }
    }
}