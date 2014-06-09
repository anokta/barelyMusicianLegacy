using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PercussiveInstrument : Instrument
{
    public AudioClip[] samples;

    public float rootPitch;


    void Start()
    {
        for (int i = 0; i < samples.Length; ++i)
        {
            audibles.Add(new Impulse(new Sampler(samples[i], false, rootPitch)));
        }
    }

    // TODO: Note structure should be added!
    public override void AddNote(float pitch)
    {
        for (int i = 0; i < samples.Length; ++i)
        {
            float targetPitch = rootPitch * Mathf.Pow(1.0594f, i);

            if (targetPitch == pitch)
            {
                audibles[i].Pitch = rootPitch;
                audibles[i].NoteOn();
            }
        }
    }

    public override void RemoveNote(float pitch)
    {
    }
}