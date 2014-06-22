using UnityEngine;
using System.Collections;

public class SamplerInstrument : MelodicInstrument
{

    public AudioClip sample;
    public bool loop;

    public float attack, decay, sustain, release;

    // TODO: Remove these (restructuring needed!)
    public int rootIndex;
    Note rootNote;
    
    protected override void Start()
    {
        rootNote = new Note(rootIndex, 1.0f);

        for (int i = 0; i < voiceCount; ++i)
        {
            audibles.Add(new Voice(new Sampler(sample, loop, rootNote.Pitch), new Envelope(attack, decay, sustain, release)));
        }

        base.Start();
    }

}
