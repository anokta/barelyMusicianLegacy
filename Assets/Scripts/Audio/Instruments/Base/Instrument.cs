﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Instrument : MonoBehaviour
{
    // Instrument Voices
    protected List<Voice> voices;

    // Effects
    protected List<AudioEffect> effects;

    // Master volume
    [SerializeField]
    [Range(0f, 1f)]
    public float MasterVolume = 1.0f;

    // Audio output
    protected AudioSource audioSource;


    protected virtual void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.Stop();

        voices = new List<Voice>();
        effects = new List<AudioEffect>();
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
            data[i] = MasterVolume * output;

            // If stereo, copy the mono data to each channel
            if (channels == 2) data[i + 1] = data[i];
        }

        foreach (AudioEffect effect in effects)
        {
            effect.Process(ref data);
        }
    }

    public virtual void RemoveAllNotes()
    {
        foreach (Voice voice in voices)
        {
            voice.StopImmediately();
        }
    }

    public abstract void NoteOn(Note note);
    public abstract void NoteOff(Note note);
}
