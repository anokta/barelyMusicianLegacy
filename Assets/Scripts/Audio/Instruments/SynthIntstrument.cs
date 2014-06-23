using UnityEngine;
using System.Collections;

namespace BarelyAPI.Musician
{
    public class SynthIntstrument : MelodicInstrument
    {
        public Oscillator.OSCType oscType;

        public float attack, decay, sustain, release;

        protected override void Start()
        {
            for (int i = 0; i < voiceCount; ++i)
            {
                voices.Add(new Voice(new Oscillator(oscType), new Envelope(attack, decay, sustain, release)));
            }

            effects.Add(new Distortion(1.0f));

            base.Start();
        }
    }
}