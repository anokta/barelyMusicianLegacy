using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class StringsMicroGenerator : MicroGenerator
    {
        int lastNote;

        public StringsMicroGenerator(Sequencer sequencer)
            : base(sequencer)
        {
            lastNote = 0;
        }

        protected override void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            if (section == SectionType.CHORUS || section == SectionType.BRIDGE)
            {
                line.Add(new NoteMeta(harmonic, 0.0f, 0.95f, 0.85f));
                if (RandomNumber.NextFloat() < 0.1f)
                    line.Add(new NoteMeta(harmonic + 4, 0.5f, 0.45f, 0.75f));
            }
        }
    }
}