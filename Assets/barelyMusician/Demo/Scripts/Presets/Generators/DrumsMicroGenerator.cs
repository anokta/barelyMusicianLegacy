using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class DrumsMicroGenerator : MicroGenerator
    {

        public DrumsMicroGenerator(SequencerState sequencerState)
            : base(sequencerState)
        {
        }

        public override List<NoteMeta> GenerateLine(char section, int bar, int harmonic)
        {
            line = new List<NoteMeta>();

            for (int i = 0; i < LineLength * 2; ++i)
            {
                float offset = i / (2.0f * LineLength);
                float duration = 1.0f / (2.0f * LineLength);

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
            }

            return line;
        }
    }
}