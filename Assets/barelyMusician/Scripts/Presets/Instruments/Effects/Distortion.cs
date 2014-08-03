using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Distortion : AudioEffect
    {
        // Distortion level
        private float level;
        public float Level
        {
            get { return level; }
            set { level = value; }
        }

        public Distortion(float distortionLevel = 1.0f)
        {
            level = distortionLevel;
        }

        public override void Apply(TimbreProperties timbreProperties)
        {
            level = Mathf.Max(1.0f, (0.1f * timbreProperties.Brightness + 0.9f * timbreProperties.Tense) * 10.0f);
        }

        public override float Process(float sample)
        {
            return Mathf.Clamp(sample * level, -1.0f, 1.0f) / level;
        }
    }
}