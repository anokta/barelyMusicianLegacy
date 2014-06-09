using UnityEngine;
using System.Collections;

public class Impulse : Audible {

    // Impulse (sample) trigger
    bool trigger;

    public Impulse(Sampler sampler, float gain = 1.0f)
    {
        sonic = sampler;

        Gain = gain;

        trigger = false;
    }

    public override void NoteOn()
    {
        sonic.Reset();

        trigger = true;
    }

    public override void NoteOff()
    {
        trigger = false;
    }

    public override float ProcessNext()
    {
        if (trigger) return gain * sonic.Next();
        else return 0.0f;
    }
}
