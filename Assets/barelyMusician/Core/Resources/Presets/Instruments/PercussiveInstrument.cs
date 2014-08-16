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

        int rootIndex;

        public PercussiveInstrument(InstrumentMeta meta)
            : base(meta)
        {
            rootIndex = meta.RootIndex;
        }

        public override void SetInstrumentProperties(InstrumentMeta meta)
        {
            base.SetInstrumentProperties(meta);

            if (voices.Count != meta.VoiceCount)
            {
                voices.Clear();

                for (int i = 0; i < meta.Samples.Length; ++i)
                {
                    voices.Add(new Voice(new Sampler(meta.Samples[i], false, new Note(meta.RootIndex).Pitch), new Envelope(0.0f, 0.0f, 1.0f, (meta.Sustained || meta.Samples[i] == null) ? 0.0f : (meta.Samples[i].length / meta.Samples[i].channels))));
                }
            }
            else
            {
                for (int i = 0; i < meta.Samples.Length; ++i)
                {
                    ((Sampler)voices[i].Ugen).Sample = meta.Samples[i];
                    ((Sampler)voices[i].Ugen).RootFrequency = new Note(meta.RootIndex).Pitch;
                    voices[i].Envelope.Release = meta.Sustained ? 0.0f : (meta.Samples[i].length / meta.Samples[i].channels);
                }
            }
        }

        // TODO: Note structure should be restructured!
        protected override void noteOn(Note note)
        {
            int index = (int)(note.Index - rootIndex) / 12;
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
                int index = (int)(note.Index - rootIndex);
                if (index >= 0 && index < voices.Count)
                {
                    voices[index].Stop();
                }
            }
        }
    }

    public enum DRUM_KIT { Kick = 0, Snare = 1, Hihat = 2, Cymbal = 3 }; 
}