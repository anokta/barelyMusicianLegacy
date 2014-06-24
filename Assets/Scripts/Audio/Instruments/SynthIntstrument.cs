using UnityEngine;
using System.Collections;

namespace BarelyAPI.Musician
{
    public class SynthIntstrument : MelodicInstrument
    {
        [SerializeField]
        private Oscillator.OSCType oscType;
        public Oscillator.OSCType OscType
        {
            get { return oscType; }
            set
            {
                foreach (Voice voice in voices)
                {
                    ((Oscillator)voice.Ugen).Type = oscType = value;
                }
            }
        }

        protected override void initialize()
        {
            for (int i = 0; i < voiceCount; ++i)
            {
                voices.Add(new Voice(new Oscillator(oscType), new Envelope(attack, decay, sustain, release)));
            }
        }
    }
}