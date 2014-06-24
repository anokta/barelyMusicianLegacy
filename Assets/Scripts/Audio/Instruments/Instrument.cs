using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyMusician
{
    public abstract class Instrument : MonoBehaviour
    {
        // Instrument Voices
        protected List<Voice> voices;

        // Envelope properties
        [SerializeField]
        protected float attack, decay, sustain, release;
        public float Attack
        {
            get { return attack; }
            set
            {
                foreach (Voice voice in voices)
                {
                    voice.Envelope.Attack = attack = value;
                }
            }
        }
        public float Decay
        {
            get { return decay; }
            set
            {
                foreach (Voice voice in voices)
                {
                    voice.Envelope.Decay = decay = value;
                }
            }
        }
        public float Sustain
        {
            get { return sustain; }
            set
            {
                foreach (Voice voice in voices)
                {
                    voice.Envelope.Sustain = sustain = value;
                }
            }
        }
        public float Release
        {
            get { return release; }
            set
            {
                foreach (Voice voice in voices)
                {
                    voice.Envelope.Release = release = value;
                }
            }
        }

        // Effects
        protected List<AudioEffect> effects;

        // Master volume (dB)
        [SerializeField] // TODO delete SerializeFields later
        protected float volume;
        public float Volume
        {
            get { return (volume != 0) ? 20.0f * Mathf.Log10(volume) : -70.0f; }
            set { volume = (value > -70.0f) ? Mathf.Pow(10, 0.05f * value) : 0.0f; }
        }

        // Audio output
        protected AudioSource audioSource;


        protected virtual void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.hideFlags = HideFlags.HideInInspector;
            audioSource.Stop();

            voices = new List<Voice>();
            effects = new List<AudioEffect>();

            initialize();
        }

        protected virtual void Start()
        {
            audioSource.Play();
        }

        protected virtual void OnAudioFilterRead(float[] data, int channels)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                // Fill the buffer
                float output = 0.0f;
                foreach (Voice voice in voices)
                {
                    output += voice.ProcessNext();
                }
                data[i] = volume * output;

                // If stereo, copy the mono data to each channel
                if (channels == 2) data[i + 1] = data[i];
            }

            foreach (AudioEffect effect in effects)
            {
                effect.Process(ref data);
            }
        }

        protected abstract void initialize();
        protected abstract void noteOn(Note note);
        protected abstract void noteOff(Note note);

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
    }
}
