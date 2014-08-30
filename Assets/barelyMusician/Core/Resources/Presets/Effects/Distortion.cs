// ----------------------------------------------------------------------
//   Adaptive music composition engine implementation for interactive systems.
//
//     Copyright 2014 Alper Gungormusler. All rights reserved.
//
// ------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Distortion : AudioEffect
    {
        // Distortion level
        private float level, levelApplied;
        public float Level
        {
            get { return level; }
            set { level = value; }
        }

        public Distortion(float distortionLevel)
        {
            level = levelApplied = distortionLevel;
        }

        public Distortion()
            : this(4.0f)
        {
        }

        public override void Apply(TimbreProperties timbreProperties)
        {
            levelApplied = Mathf.Max(1.0f, (0.1f * timbreProperties.Brightness + 0.9f * timbreProperties.Tense) * level);
        }

        public override float Process(float sample)
        {
            return Mathf.Clamp(sample * levelApplied, -1.0f, 1.0f) / levelApplied;
        }
    }
}