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
                int noteIndex = (i % 2 == 0) ? harmonic : harmonic + ModeGenerator.SCALE_LENGTH;
                float volume = (i % 2 == 0) ? 0.9f : 0.6f;
                if (i == 0) volume = 1.0f;
                if (i != 1)
                    line.Add(new NoteMeta(noteIndex, 0.5f * i / LineLength, 1.0f / LineLength, volume));
            }
        }
    }
}