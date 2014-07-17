using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class PercussiveInstrument : Instrument
    {
        public bool sustained;

        public AudioClip[] samples;

        public int rootIndex;
        Note rootNote;
        
        protected override void initialize()
        {
            rootNote = new Note(rootIndex, 1.0f);

            for (int i = 0; i < samples.Length; ++i)
            {
                voices.Add(new Voice(new Sampler(samples[i], false, rootNote.Pitch), new Envelope(0.0f, 0.0f, 1.0f, sustained ? 0.0f : samples.Length)));
                voices[voices.Count - 1].Pitch = rootNote.Pitch;
            }
        }

        // TODO: Note structure should be restructured!
        protected override void noteOn(Note note)
        {
            int index = (int)(note.Index - rootNote.Index);
            if (index >= 0 && index < voices.Count)
            {
                voices[index].Gain = note.Loudness;
                voices[index].Start();
            }
        }

        protected override void noteOff(Note note)
        {
            if (sustained)
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