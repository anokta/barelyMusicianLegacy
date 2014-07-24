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
            set { loudness = 0.4f + 0.6f * value; }
        }

        float noteOnset;
        public float NoteOnsetMultiplier
        {
            get { return noteOnset; }
            set { noteOnset = 0.5f + 1.5f * value; }
        }

        ModeGenerator mode;
        public float Mode
        {
            set { mode.GenerateScale(value); }
        }



        // 0.0f - 1.0f
        public float loudnessVariance;

        // 0.0f - 1.0f
        public float articulationVariance;

        // 0.0f - 1.0f
        public float harmonicComplexity;

        // 0.0f - 1.0f
        public float harmonicCurve;

        // 0.0f - 1.0f
        public float pitchHeight;


        public float fundamentalKey;

        
        public Conductor()
        {
            mode = new SimpleModeGenerator();
        }

        public float GetNote(float index)
        {
            return fundamentalKey + mode.GetNoteOffset(index);
        }
    }
}