using UnityEngine;
using System.Collections;

public class SynthIntstrument : MelodicInstrument
{

    public Oscillator.OSCType oscType;

    public float attack, decay, sustain, release;


    void Start()
    {
        for (int i = 0; i < voiceCount; ++i)
        {
            audibles.Add(new Voice(new Oscillator(oscType), new Envelope(attack, decay, sustain, release)));
        }
    }

}
