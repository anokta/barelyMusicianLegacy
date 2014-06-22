using UnityEngine;
using System.Collections;

public class Voice : Audible
{
    Envelope envelope;

    enum VoiceType { SYNTH, SAMPLER }
    VoiceType type;

    public Voice(UGen sonic, Envelope envelope, float gain = 1.0f)
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

        envelope.State = Envelope.EnvelopeState.ATTACK;
    }

    public override void NoteOff()
    {
        envelope.State = Envelope.EnvelopeState.RELEASE;
    }

    public override float ProcessNext()
    {
        return gain * envelope.Next() * sonic.Next();
    }
}
