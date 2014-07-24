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

        public SynthInstrument(OscillatorType oscType, Envelope envelope, float volume = -10.0f, int voiceCount = 16)
            : base(volume)
        {
            for (int i = 0; i < voiceCount; ++i)
            {
                voices.Add(new Voice(new Oscillator(oscType), new Envelope(envelope.Attack, envelope.Decay, envelope.Sustain, envelope.Release)));
            }

            StopAllNotes();
        }
    }
}