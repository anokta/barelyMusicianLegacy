using UnityEngine;
using System.Collections;

namespace BarelyMusician
{
    public class SamplerInstrument : MelodicInstrument
    {
        public AudioClip sample;
        public bool loop;

        // TODO: Remove these (restructuring needed!)
        public int rootIndex;
        Note rootNote;

        protected override void initialize()
        {
            rootNote = new Note(rootIndex, 1.0f);

            for (int i = 0; i < voiceCount; ++i)
            {
                voices.Add(new Voice(new Sampler(sample, loop, rootNote.Pitch), new Envelope(attack, decay, sustain, release)));
            }
        }
    }
}
