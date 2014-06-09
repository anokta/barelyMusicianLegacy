using UnityEngine;
using System.Collections;

public class SamplerInstrument : MelodicInstrument
{

    public AudioClip sample;
    public bool loop;

    public float attack, decay, sustain, release;


    void Start()
    {
        for (int i = 0; i < voiceCount; ++i)
        {
            audibles.Add(new Voice(new Sampler(sample, loop), new Envelope(attack, decay, sustain, release)));
        }
    }

}
