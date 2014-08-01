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
            level = Mathf.Max(1.0f, (0.25f * timbreProperties.Brightness + 0.75f * timbreProperties.Tense) * 5.0f);
        }

        public override float Process(float sample)
        {
            return sample * level;
        }
    }
}