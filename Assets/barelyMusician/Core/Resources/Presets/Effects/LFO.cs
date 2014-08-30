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
    public class LFO : AudioEffect
    {
        // Distortion level
        private Oscillator osc;
        public float Frequency
        {
            get { return osc.Frequency; }
            set { osc.Frequency = value; }
        }

        public LFO(float frequency)
        {
            osc = new Oscillator(OscillatorType.SINE);
            Frequency = frequency;
        }

        public LFO()
            : this(5.0f)
        {
        }

        public override void Apply(TimbreProperties timbreProperties)
        {
            Frequency = 10.0f * (0.5f * timbreProperties.Brightness + 0.5f * timbreProperties.Tense);
        }

        public override float Process(float sample)
        {
            return sample * osc.Next();
        }
    }
}