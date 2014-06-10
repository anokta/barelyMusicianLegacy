using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PercussiveInstrument : Instrument
{
    public bool sustained;

    public AudioClip[] samples;

    public int rootIndex;
    Note rootNote;

    void Start()
    {
        rootNote = new Note(rootIndex, 1.0f);

        for (int i = 0; i < samples.Length; ++i)
        {
            audibles.Add(new Impulse(new Sampler(samples[i], false, rootNote.Pitch)));
            audibles[audibles.Count - 1].Pitch = rootNote.Pitch;
        }
    }

    // TODO: Note structure should be restructured!
    public override void AddNote(Note note)
    {
        int index = note.Index - rootNote.Index;
        if (index >= 0 && index < audibles.Count)
        {
            audibles[index].Volume = note.Velocity;
            audibles[index].NoteOn();
        }   
    }

    public override void RemoveNote(Note note)
    {
        if (sustained)
        {
            int index = note.Index - rootNote.Index;
            if (index >= 0 && index < audibles.Count)
            {
                audibles[index].NoteOff();
            }   
        }
    }
}