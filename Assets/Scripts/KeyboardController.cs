using UnityEngine;
using System.Collections;
using BarelyAPI.Musician;

public class KeyboardController : MonoBehaviour
{
    public int fundamentalIndex = 36; //261.63f;

    Instrument instrument;

    KeyCode[] keys = { 
                         KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.E, KeyCode.D, KeyCode.F, KeyCode.T, KeyCode.G, KeyCode.Y, KeyCode.H, KeyCode.U, KeyCode.J, KeyCode.K, KeyCode.O, KeyCode.L 
                     };


    void Awake()
    {
        instrument = GetComponent<Instrument>();
    }

    void Update()
    {
        // octave up-down
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fundamentalIndex = Mathf.Max(0, fundamentalIndex-12);
            instrument.StopAllNotes();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            fundamentalIndex = Mathf.Min(Note.notes.Length - 12, fundamentalIndex + 12);
            instrument.StopAllNotes();
        }

        // keys
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyUp(keys[i]))
            {
                instrument.PlayNote(new Note(fundamentalIndex+i, 0.0f));
            }
            else if (Input.GetKeyDown(keys[i]))
            {
                instrument.PlayNote(new Note(fundamentalIndex + i, 1.0f));
            }
        }
    }
}