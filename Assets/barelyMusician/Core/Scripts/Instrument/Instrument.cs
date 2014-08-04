using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public abstract class Instrument
    {
        public static float MIN_VOLUME = -70.0f;
        const float MIN_ONSET = 0.01f;

        // Instrument Voices
        protected List<Voice> voices;

        // Effects
        protected List<AudioEffect> effects;
        public List<AudioEffect> Effects
        {
            get { return effects; }
        }

        // Envelope properties
        public float Attack
        {
            get { return voices[0].Envelope.Attack; }
            set
            {
                float attack = Mathf.Max(MIN_ONSET, value);

                foreach (Voice voice in voices)
                {
                    voice.Envelope.Attack = attack;
                }
            }
        }
        public float Decay
        {
            get { return voices[0].Envelope.Decay; }
            set
            {
                foreach (Voice voice in voices)
                {
                    voice.Envelope.Decay = value;
                }
            }
        }
        public float Sustain
        {
            get { return voices[0].Envelope.Sustain; }
            set
            {
                foreach (Voice voice in voices)
                {
                    voice.Envelope.Sustain = value;
                }
            }
        }
        public float Release
        {
            get { return voices[0].Envelope.Release; }
            set
            {
                foreach (Voice voice in voices)
                {
                    voice.Envelope.Release = value;
                }
            }
        }

        protected float volume;
        public float Volume
        {
            get { return (volume != 0.0f) ? 20.0f * Mathf.Log10(volume) : MIN_VOLUME; }
            set { volume = (value > MIN_VOLUME) ? Mathf.Pow(10, 0.05f * value) : 0.0f; }
        }

        public Instrument(float volume = 0.0f)
        {
            voices = new List<Voice>();
            effects = new List<AudioEffect>();

            Volume = volume;
        }

        public void AddEffect(AudioEffect effect)
        {
            effects.Add(effect);
        }

        public float ProcessNext()
        {
            float output = 0.0f;

            foreach (Voice voice in voices)
            {
                output += voice.ProcessNext();
            }

            foreach (AudioEffect effect in effects)
            {
                if (effect.Enabled)
                    output = effect.Process(output);
            }

            return output * volume;
        }

        public virtual void PlayNote(Note note)
        {
            if (note.IsNoteOn)
                noteOn(note);
            else
                noteOff(note);
        }

        public virtual void StopAllNotes()
        {
            foreach (Voice voice in voices)
            {
                voice.StopImmediately();
            }
        }

        protected abstract void noteOn(Note note);
        protected abstract void noteOff(Note note);
    }
}