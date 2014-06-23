using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PercussiveInstrument : Instrument
{
    public bool sustained;

    public AudioClip[] samples;

    public int rootIndex;
    Note rootNote;

    protected override void Start()
    {
        rootNote = new Note(rootIndex, 1.0f);

        for (int i = 0; i < samples.Length; ++i)
        {
            voices.Add(new Voice(new Sampler(samples[i], false, rootNote.Pitch), new Envelope(0.0f, 0.0f, 1.0f, samples.Length)));
            voices[voices.Count - 1].Pitch = rootNote.Pitch;
        }

        base.Start();
    }

    // TODO: Note structure should be restructured!
    public override void NoteOn(Note note)
    {
        int index = note.Index - rootNote.Index;
        if (index >= 0 && index < voices.Count)
        {
            voices[index].Gain = note.Velocity;
            voices[index].Start();
        }
    }

    public override void NoteOff(Note note)
    {
        if (sustained)
        {
            int index = note.Index - rootNote.Index;
            if (index >= 0 && index < voices.Count)
            {
                voices[index].Stop();
            }
        }
    }
}