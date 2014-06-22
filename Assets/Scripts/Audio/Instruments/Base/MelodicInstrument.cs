using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MelodicInstrument : Instrument
{
    [SerializeField]
    [Range(1, 32)]
    public int voiceCount = 1;

    LinkedList<Audible> activeList, freeList;

    protected virtual void Start()
    {
        freeList = new LinkedList<Audible>(audibles);
        activeList = new LinkedList<Audible>();
    }

    // TODO: Refactoring needed!
    public override void AddNote(Note note)
    {
        Audible audible;
        
        // is any voice available?
        if (freeList.Count > 0)
        {
            audible = freeList.First.Value;

            freeList.RemoveFirst();
            activeList.AddLast(audible);
        }
        else
        {   
            audible = activeList.First.Value;
        }

        audible.Pitch = note.Pitch;
        audible.Volume = note.Velocity;
        audible.NoteOn();
    }

    public override void RemoveNote(Note note)
    {
        foreach (Audible audible in activeList)
        {
            if (audible.Pitch == note.Pitch)
            {
                audible.NoteOff();
                activeList.Remove(audible);
                freeList.AddLast(audible);

                break;
            }
        }
    }

    public override void RemoveAllNotes()
    {
        activeList.Clear();
        freeList = new LinkedList<Audible>(audibles);

        base.RemoveAllNotes();
    }
}