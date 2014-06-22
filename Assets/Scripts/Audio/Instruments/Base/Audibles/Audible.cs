using UnityEngine;
using System.Collections;

public abstract class Audible
{
    // Volume
    protected float gain;
    public float Volume
    {
        get { return gain; }
        set { gain = value; }
    }

    // Frequency
    public float Pitch
    {
        get { return sonic.Frequency; }
        set { sonic.Frequency = value; }
    }

    // Sound generator
    protected UGen sonic;

    // Is being used or not?
    public abstract bool IsFree();

    // Note onset
    public abstract void NoteOn();
    public abstract void NoteOff();

    // Process next sample
    public abstract float ProcessNext();
}