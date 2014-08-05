using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class SimpleMicroGenerator : MicroGenerator
    {
        int[] pattern = { 0, 2, 4, 7, 4 };

        public SimpleMicroGenerator(SequencerState sequencerState)
            : base(sequencerState)
        {
        }

        protected override void generateLine(char section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            line.Add(new NoteMeta(harmonic + pattern[0], 0.0f, 1.0f / 4.0f, 1.0f));
            line.Add(new NoteMeta(harmonic + pattern[1], 1.0f / 4.0f, 1.0f / 4.0f, 0.9f));
            line.Add(new NoteMeta(harmonic + pattern[2], 2.0f / 4.0f, 1.0f / 4.0f, 0.95f));
            line.Add(new NoteMeta(harmonic + pattern[3], 3.0f / 4.0f, 1.0f / 8.0f, 0.9f));
            line.Add(new NoteMeta(harmonic + pattern[4], 3.5f / 4.0f, 1.0f / 8.0f, 0.8f));
        }
    }
}