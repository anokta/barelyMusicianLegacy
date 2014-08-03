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

        public SamplerInstrument(AudioClip sample, Envelope envelope, float volume = 0.0f, int rootIndex = 0, bool loop = false, int voiceCount = 16)
            : base(volume)
        {
            for (int i = 0; i < voiceCount; ++i)
            {
                voices.Add(new Voice(new Sampler(sample, loop, new Note(rootIndex).Pitch), new Envelope(envelope.Attack, envelope.Decay, envelope.Sustain, envelope.Release)));
            }

            StopAllNotes();
        }
    }
}
