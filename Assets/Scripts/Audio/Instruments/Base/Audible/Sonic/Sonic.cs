using UnityEngine;
using System.Collections;

public abstract class Sonic
{
    // Frequency (Hz)
    protected float frequency;
    public float Frequency
    {
        get { return frequency; }
        set { frequency = value; }
    }

    // Internal clock
    protected float phase;

    // Final output
    protected float output;

    // Compute next sample
    public abstract float Next();

    protected Sonic()
    {
        Reset();
    }

    public virtual void Reset()
    {
        phase = 0.0f;
        output = 0.0f;
    }
}