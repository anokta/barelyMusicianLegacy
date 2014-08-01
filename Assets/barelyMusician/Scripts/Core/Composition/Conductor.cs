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

        // Musical mode
        ModeGenerator mode;
        public float Mode
        {
            set { mode.GenerateScale(value); }
        }

        // Tempo
        float tempoMult;
        public float TempoMultiplier
        {
            get { return tempoMult; }
            set { tempoMult = 0.875f + 0.25f * value; }
        }

        // Articulation
        float articulationMult;
        public float ArticulationMultiplier
        {
            get { return articulationMult; }
            set { articulationMult = 0.5f + value; }
        }

        // Loudness
        float loudnessMult;
        public float LoudnessMultiplier
        {
            get { return loudnessMult; }
            set { loudnessMult = 0.4f + 0.6f * value; }
        }

        // Note onset
        float noteOnsetMult;
        public float NoteOnsetMultiplier
        {
            get { return noteOnsetMult; }
            set { noteOnsetMult = 0.25f + 1.75f * value; }
        }

        // Articulation variance
        float articulationVariance;
        public float ArticulationVariance
        {
            get { return articulationVariance; }
            set { articulationVariance = 0.2f * value; }
        }

        // Loudness Variance
        float loudnessVariance;
        public float LoudnessVariance
        {
            get { return loudnessVariance; }
            set { loudnessVariance = 0.25f * value; }
        }

        // 0.0f - 1.0f
        //public float harmonicComplexity;

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
            set { pitchHeight = 4.0f * value - 2.0f; }
        }

        public Conductor(float key)
        {
            fundamentalKey = key;

            mode = new SimpleModeGenerator();
        }

        public void SetParameters(float energy, float stress)
        {
            TempoMultiplier = energy;
            ArticulationMultiplier = 1.0f - energy;
            LoudnessMultiplier = energy;
            NoteOnsetMultiplier = 1.0f - energy;
            ArticulationVariance = energy;

            //harmonicComplexity = stress;
            Mode = stress;

            PitchHeight = energy * 0.25f + (1.0f - stress) * 0.75f;
            LoudnessVariance = (energy + stress) / 2.0f;
            HarmonicCurve = (stress > 0.5f) ? (0.75f * (1.0f - stress) + 0.25f * (1.0f - energy)) : 1.0f;

        }

        public void ApplyPerformerTransformation(Performer performer)
        {
            performer.Onset *= NoteOnsetMultiplier;
        }

        public NoteMeta TransformNote(NoteMeta meta)
        {
            float index = getNote(Mathf.RoundToInt(harmonicCurve) != 0 ? Mathf.RoundToInt(harmonicCurve) * meta.Index : meta.Index + Mathf.RoundToInt(pitchHeight) / 2 * ModeGenerator.SCALE_LENGTH);
            float offset = meta.Offset;
            float duration = Mathf.Max(0.0f, RandomNumber.NextNormal(meta.Duration * articulationMult, meta.Duration * articulationMult * articulationVariance));
            float loudness = Mathf.Max(0.0f, RandomNumber.NextNormal(meta.Loudness * loudnessMult, meta.Loudness * loudnessMult * loudnessVariance));

            return new NoteMeta(index, offset, duration, loudness);
        }

        float getNote(float index)
        {
            return fundamentalKey + mode.GetNoteOffset(index);
        }
    }
}