using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public abstract class MicroGenerator
    {
        SequencerState state;
        protected int LineLength
        {
            get { return state.BeatCount; }
        }
        protected int ProgressionLength
        {
            get { return state.BarCount; }
        }

        Dictionary<SectionType, List<NoteMeta>[]> lines;

        protected MicroGenerator(SequencerState sequencerState)
        {
            state = sequencerState;
        }

        public List<NoteMeta> GetLine(SectionType section, int bar, int harmonic)
        {
            List<NoteMeta>[] lineSection = null;
            if(!lines.TryGetValue(section, out lineSection))
                lines[section] = lineSection = new List<NoteMeta>[ProgressionLength];

            if(lineSection[bar] == null)
            {
                lineSection[bar] = new List<NoteMeta>();
                generateLine(section, bar, harmonic, ref lineSection[bar]);
            }

            return lineSection[bar];
        }

        public virtual void Restart()
        {
            lines = new Dictionary<SectionType, List<NoteMeta>[]>();
        }

        protected abstract void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line);
    }
}