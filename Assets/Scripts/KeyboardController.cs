using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour
{

    public float fundamental = 261.63f;

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
            fundamental /= 2.0f;
            instrument.RemoveAllNotes();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            fundamental *= 2.0f;
            instrument.RemoveAllNotes();
        }

        // keys
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyUp(keys[i]))
            {
                instrument.RemoveNote(fundamental * Mathf.Pow(1.0594f, i));
            }
            else if (Input.GetKeyDown(keys[i]))
            {
                instrument.AddNote(fundamental * Mathf.Pow(1.0594f, i));
            }
        }
    }
}