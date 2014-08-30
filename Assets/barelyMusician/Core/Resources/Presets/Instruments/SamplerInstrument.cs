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
    public class SamplerInstrument : MelodicInstrument
    {
        public bool Loop
        {
            get { return ((Sampler)voices[0].Ugen).Loop; }
            set    
            {
                foreach (Voice voice in voices)
                {
                    ((Sampler)voice.Ugen).Loop = value;
                }
            }
        }

        public SamplerInstrument(InstrumentMeta meta)
            : base(meta)
        {
        }

        public override void SetInstrumentProperties(InstrumentMeta meta)
        {
            base.SetInstrumentProperties(meta);

            if (voices.Count != meta.VoiceCount)
            {
                voices.Clear();

                for (int i = 0; i < meta.VoiceCount; ++i)
                {
                    voices.Add(new Voice(new Sampler(meta.Sample, meta.Sustained, new Note(meta.RootIndex).Pitch), new Envelope(meta.Attack, meta.Decay, meta.Sustain, meta.Release)));
                }
            }
            else
            {
                foreach (Voice voice in voices)
                {
                    ((Sampler)voice.Ugen).Sample = meta.Sample;
                    ((Sampler)voice.Ugen).Loop = meta.Sustained;
                    ((Sampler)voice.Ugen).RootFrequency = new Note(meta.RootIndex).Pitch;
                    voice.Envelope.Attack = meta.Attack;
                    voice.Envelope.Decay = meta.Decay;
                    voice.Envelope.Sustain = meta.Sustain;
                    voice.Envelope.Release = meta.Release;
                }
            }
        }
    }
}
