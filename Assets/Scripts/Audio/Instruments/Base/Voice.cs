using UnityEngine;
using System.Collections;

namespace BarelyAPI.Musician
{
    public class Voice
    {
        // Volume
        float gain;
        public float Gain
        {
            get { return gain; }
            set { gain = value; }
        }

        // Frequency
        public float Pitch
        {
            get { return ugen.Frequency; }
            set { ugen.Frequency = value; }
        }

        // Envelope
        Envelope envelope;
        public Envelope Envelope
        {
            get { return envelope; }
        }

        // Sound generator
        UGen ugen;

        public Voice(UGen soundGenerator, Envelope soundEnvelope, float gain = 1.0f)
        {
            ugen = soundGenerator;
            envelope = soundEnvelope;

            Gain = gain;
        }

        public void Start()
        {
            ugen.Reset();

            envelope.Start();
        }

        public void Stop()
        {
            envelope.Stop();
        }

        public void StopImmediately()
        {
            envelope.Reset();

            ugen.Reset();
        }

        public float ProcessNext()
        {
            return gain * envelope.Next() * ugen.Next();
        }
    }
}
