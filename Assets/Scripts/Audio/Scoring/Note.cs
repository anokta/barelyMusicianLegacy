using UnityEngine;
using System.Collections;

public class Note {

    // C0
    public static float rootFrequency = 16.35f;

    // Notes from C0 to B7
    public static float[] notes;

    public static void Initialize()
    {
        notes = new float[12 * 8];

        float currentFrequency = rootFrequency;
        for (int i = 0; i < notes.Length; ++i)
        {
            notes[i] = currentFrequency;
            currentFrequency *= 1.0594f;
        }
    }

    public float Pitch;
    public int Index;
    public float Velocity;

    public Note(int index, float velocity = 1.0f)
    {
        Index = index;
        Pitch = notes[Index];

        Velocity = velocity;
    }

    public Note(float frequency, float velocity = 1.0f)
    {
        for (int i = 0; i < notes.Length; ++i)
        {
            if (Mathf.Abs(notes[i] - frequency) < 0.01f)
            {
                Index = i;
                break;
            }
        }
        Pitch = notes[Index];

        Velocity = velocity;
    }
	
}
