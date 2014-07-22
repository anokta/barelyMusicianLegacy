using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class PercussiveInstrument : Instrument
    {
        public bool Sustained
        {
            get { return voices[0].Envelope.Release == 0.0f; }
            set
            {
                foreach (Voice voice in voices)
                {
                    voice.Envelope.Release = value ? 0.0f : ((Sampler)voice.Ugen).SampleLength;
                }
            }
        }

        Note rootNote;

        public PercussiveInstrument(AudioClip[] samples, float volume = 0.0f, bool sustained = false, int rootIndex = 0)
            : base(volume)
        {
            rootNote = new Note(rootIndex);

            for (int i = 0; i < samples.Length; ++i)
            {
                voices.Add(new Voice(new Sampler(samples[i], false, rootNote.Pitch), new Envelope(0.0f, 0.0f, 1.0f, sustained ? 0.0f : samples.Length)));
            }
        }

        // TODO: Note structure should be restructured!
        protected override void noteOn(Note note)
        {
            int index = (int)(note.Index - rootNote.Index) / 12;
            if (index >= 0 && index < voices.Count)
            {
                voices[index].Gain = note.Loudness;
                voices[index].Start();
            }
        }

        protected override void noteOff(Note note)
        {
            if (Sustained)
            {
                int index = (int)(note.Index - rootNote.Index);
                if (index >= 0 && index < voices.Count)
                {
                    voices[index].Stop();
                }
            }
        }
    }
}