using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class SynthInstrument : MelodicInstrument
    {
        public OscillatorType OscType
        {
            get { return ((Oscillator)voices[0].Ugen).Type; }
            set
            {
                foreach (Voice voice in voices)
                {
                    ((Oscillator)voice.Ugen).Type = value;
                }
            }
        }

        public SynthInstrument(InstrumentMeta meta)
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
                    voices.Add(new Voice(new Oscillator(meta.OscType), new Envelope(meta.Attack, meta.Decay, meta.Sustain, meta.Release)));
                }
            }
            else
            {
                foreach (Voice voice in voices)
                {
                    ((Oscillator)voice.Ugen).Type = meta.OscType;
                    voice.Envelope.Attack = meta.Attack;
                    voice.Envelope.Decay = meta.Decay;
                    voice.Envelope.Sustain = meta.Sustain;
                    voice.Envelope.Release = meta.Release;
                }
            }
        }
    }
}