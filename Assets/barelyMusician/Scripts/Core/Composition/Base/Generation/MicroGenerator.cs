using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public abstract class MicroGenerator
    {
        protected List<NoteMeta> line;

        SequencerState state;
        protected int LineLength
        {
            get { return state.BeatCount; }
        }
        protected int ProgressionLength
        {
            get { return state.BarCount; }
        }

        protected MicroGenerator(SequencerState sequencerState)
        {
            state = sequencerState;
            line = new List<NoteMeta>();
        }

        public abstract List<NoteMeta> GenerateLine(char section, int bar, int harmonic);
    }
}