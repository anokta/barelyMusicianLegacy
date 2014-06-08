﻿using UnityEngine;
using System.Collections;

public class Envelope
{
    // Final amplitude (0. - 1.)
    public float Multiplier;
    float multiplierLast;

    // Attack (ms)
    float attack;
    public float Attack
    {
        get { return 1.0f / attack; }
        set { attack = 1.0f / value; }
    }

    // Decay (ms)
    float decay;
    public float Decay
    {
        get { return 1.0f / decay; }
        set { decay = 1.0f / value; }
    }

    // Sustain (0. - 1.)
    float sustain;
    public float Sustain
    {
        get { return sustain; }
        set { sustain = value; }
    }

    // Release (ms)
    float release;
    public float Release
    {
        get { return 1.0f / release; }
        set { release = 1.0f / value; }
    }

    // Note onset
    bool noteOn;
    public bool NoteOn
    {
        set
        {
            noteOn = value;

            if (noteOn)
            {
                changeState(EnvelopeState.ATTACK);
            }
            else
            {
                multiplierLast = Multiplier;
                changeState(EnvelopeState.RELEASE);
            }
        }
        get
        {
            return noteOn;
        }
    }

    // Envelope state
    enum EnvelopeState { ATTACK, DECAY, SUSTAIN, RELEASE, OFF };
    EnvelopeState state;

    // State progress
    float progress;


    public Envelope(float attack, float decay, float sustain, float release)
    {
        Attack = attack;
        Decay = decay;
        Sustain = sustain;
        Release = release;

        noteOn = false;
        Multiplier = multiplierLast = 0.0f;
        changeState(EnvelopeState.OFF);
    }

    public void OnNextBlock(float interval)
    {
        switch (state)
        {
            case EnvelopeState.OFF:
                break;

            case EnvelopeState.ATTACK:
                progress += interval * attack;
                Multiplier = Mathf.Lerp(0.0f, 1.0f, progress);
                if (progress >= 1.0f) changeState(EnvelopeState.DECAY);
                break;

            case EnvelopeState.DECAY:
                progress += interval * decay;
                Multiplier = Mathf.Lerp(1.0f, sustain, progress);
                if (progress >= 1.0f) changeState(EnvelopeState.SUSTAIN);
                break;

            case EnvelopeState.SUSTAIN:
                Multiplier = sustain;
                break;

            case EnvelopeState.RELEASE:
                progress += interval * release;
                Multiplier = Mathf.Lerp(multiplierLast, 0.0f, progress);
                if (progress >= 1.0) changeState(EnvelopeState.OFF);
                break;
        }
    }

    void changeState(EnvelopeState state)
    {
        progress = 0.0f;
        this.state = state;
    }
}
