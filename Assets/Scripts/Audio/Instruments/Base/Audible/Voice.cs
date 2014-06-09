using UnityEngine;
using System.Collections;

public class Voice : Audible
{
    Envelope envelope;

    enum VoiceType { SYNTH, SAMPLER }
    VoiceType type;

    public Voice(Sonic sonic, Envelope envelope, float gain = 1.0f)
    {
        this.sonic = sonic;
        if (sonic.GetType() == typeof(Oscillator)) type = VoiceType.SYNTH;
        else type = VoiceType.SAMPLER;

        this.envelope = envelope;

        Volume = gain;
    }

    public override bool IsFree()
    {
        return envelope.State == Envelope.EnvelopeState.OFF;
    }

    public override void NoteOn()
    {
        if (type == VoiceType.SAMPLER)
        {
            sonic.Reset();
        }

        envelope.NoteOn = true;
    }

    public override void NoteOff()
    {
        envelope.NoteOn = false;
    }

    public override float ProcessNext()
    {
        return gain * envelope.Next() * sonic.Next();
    }
}
