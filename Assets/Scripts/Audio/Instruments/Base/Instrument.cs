﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Instrument : MonoBehaviour
{
    // Instrument audibles
    protected List<Audible> audibles;

    // Master volume
    [SerializeField]
    [Range(0f, 1f)]
    public float MasterVolume = 1.0f;

    // Effects
    protected List<AudioEffect> effects;

    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.Stop();

        audibles = new List<Audible>();
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
            foreach (Audible audible in audibles)
            {
                output += audible.ProcessNext();
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
        foreach (Audible audible in audibles)
        {
            audible.NoteOff();
        }
    }

    public abstract void AddNote(Note note);
    public abstract void RemoveNote(Note note);
}
