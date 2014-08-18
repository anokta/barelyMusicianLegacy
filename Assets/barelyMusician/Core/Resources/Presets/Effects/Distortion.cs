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

        public Distortion(float distortionLevel = 10.0f)
        {
            level = levelApplied = distortionLevel;
        }

        public Distortion()
            : this(10.0f)
        {
        }

        public override void Apply(TimbreProperties timbreProperties)
        {
            levelApplied = Mathf.Max(1.0f, (0.1f * timbreProperties.Brightness + 0.9f * timbreProperties.Tense) * level);
        }

        public override float Process(float sample)
        {
            return Mathf.Clamp(sample * levelApplied, -3.0f, 3.0f) / levelApplied;
        }
    }
}