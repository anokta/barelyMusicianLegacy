using UnityEngine;
using System.Collections;
using System;

namespace BarelyAPI
{
    public class Conductor
    {
        // Key note
        float fundamentalKey;
        public float Key
        {
            get { return fundamentalKey; }
            set { fundamentalKey = value; }
        }

        // Tempo
        float tempo;
        public float TempoMultiplier
        {
            get { return tempo; }
            set { tempo = 0.875f + 0.25f * value; }
        }

        // Articulation
        float articulation;
        public float ArticulationMultiplier
        {
            get { return articulation; }
            set { articulation = 0.5f + value; }
        }

        // Loudness
        float loudness;
        public float LoudnessMultiplier
        {
            get { return loudness; }
            set { loudness = 0.4f + 0.6f * value; }
        }

        // Note onset
        float noteOnset;
        public float NoteOnsetMultiplier
        {
            get { return noteOnset; }
            set { noteOnset = 0.25f + 1.75f * value; }
        }

        // Musical mode
        ModeGenerator mode;
        public float Mode
        {
            set { mode.GenerateScale(value); }
        }

        // Loudness Variance
        float loudnessVariance;
        public float LoudnessVariance
        {
            get { return loudnessVariance; }
            set { loudnessVariance = 0.25f * value; }
        }

        // Articulation variance
        float articulationVariance;
        public float ArticulationVariance
        {
            get { return articulationVariance; }
            set { articulationVariance = 0.2f * value; }
        }

        // 0.0f - 1.0f
        public float harmonicComplexity;

        float harmonicCurve;
        public float HarmonicCurve
        {
            get { return harmonicCurve; }
            set { harmonicCurve = 2.0f * value - 1.0f; }
        }

        float pitchHeight;
        public float PitchHeight
        {
            get { return pitchHeight; }
            set { pitchHeight = 2.0f * value - 1.0f; }
        }

        public Conductor(float key)
        {
            fundamentalKey = key;

            mode = new SimpleModeGenerator();
        }

        public float GetNote(float index)
        {
            return fundamentalKey + mode.GetNoteOffset(index);
        }
    }
}