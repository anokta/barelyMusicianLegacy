using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class SimpleBassMicroGenerator : MicroGenerator
    {
        int lastNote;

        public SimpleBassMicroGenerator(Sequencer sequencer)
            : base(sequencer)
        {
            lastNote = 0;
        }

        protected override void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            for (int i = 0; i < LineLength; ++i)
            {
                lastNote += RandomNumber.NextInt(-ModeGenerator.SCALE_LENGTH / 4, ModeGenerator.SCALE_LENGTH / 2);
                lastNote = System.Math.Max(-ModeGenerator.SCALE_LENGTH / 4, System.Math.Min(ModeGenerator.SCALE_LENGTH, lastNote));

                //if ((i % 4 == 0 || RandomNumber.NextFloat() < 0.95f))// && ((section != SectionType.INTRO && section != SectionType.OUTRO) || RandomNumber.NextFloat() > 0.5f))
                //{
                line.Add(new NoteMeta(harmonic + lastNote, (float)i / LineLength, 0.5f / LineLength, RandomNumber.NextFloat(0.85f, 0.90f)));
                //}
            }
        }
    }
}