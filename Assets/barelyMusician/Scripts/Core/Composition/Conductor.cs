using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Conductor
    {
        float articulation;
        public float ArticulationMultiplier
        {
            get { return articulation; }
            set { articulation = 0.5f + value; }
        }

        float loudness;
        public float LoudnessMultiplier
        {
            get { return loudness; }
            set { loudness = 0.25f + 0.75f * value; }
        }

        float noteOnset;
        public float NoteOnsetMultiplier
        {
            get { return noteOnset; }
            set { noteOnset = 0.5f + 1.5f * value; }
        }

        public float fundamentalKey;

        public ModeGenerator mode;

        public Conductor()
        {
            mode = new SimpleModeGenerator();
            mode.GenerateScale(0.0f, -1.0f);
        }

        public float GetNote(int index)
        {
            return fundamentalKey + mode.GetNoteOffset(index);
        }
    }
}