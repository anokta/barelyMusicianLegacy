using UnityEngine;
using System.Collections;
using System;

public class Oscillator
{
    public enum OSCType { SINE, COS, SAW, SQUARE, PULSE, TRIANGLE, NOISE };
    OSCType type;
    
    float frequency;
    public float Frequency
    {
        get { return frequency; }
        set { frequency = value; }
    }

    float duty;
    public float PulseDuty
    {
        get { return duty; }
        set { duty = value; }
    }

    Func<float> oscFunc;

    float phase;
    float output;

    static float TWO_PI = 2.0f * Mathf.PI;
    static System.Random rand = new System.Random();

    public Oscillator(OSCType type)
    {
        this.type = type;

        switch (type)
        {
            case OSCType.SINE:
                oscFunc = sine;
                break;
            case OSCType.COS:
                oscFunc = cos;
                break;
            case OSCType.SAW:
                oscFunc = saw;
                break;
            case OSCType.SQUARE:
                oscFunc = square;
                break;
            case OSCType.PULSE:
                oscFunc = pulse;
                break;
            case OSCType.TRIANGLE:
                oscFunc = triangle;
                break;
            case OSCType.NOISE:
                oscFunc = noise;
                break;
        }
    }

    public float Next()
    {
        return oscFunc();
    }

    float sine()
    {
        output = Mathf.Sin(phase * TWO_PI);

        if (phase >= 1.0f) phase -= 1.0f;
        phase += frequency * AudioProperties.Interval;

        return output;
    }

    float cos()
    {
        output = Mathf.Cos(phase * TWO_PI);

        if (phase >= 1.0f) phase -= 1.0f;
        phase += frequency * AudioProperties.Interval;

        return output;
    }

    float saw()
    {
        output = phase;

        if (phase >= 1.0f) phase -= 2.0f;
        phase += frequency * AudioProperties.Interval;

        return output;
    }

    float square()
    {
        if (phase < 0.5f) output = -1.0f;
        if (phase > 0.5f) output = 1.0f;

        if (phase >= 1.0f) phase -= 1.0f;
        phase += frequency * AudioProperties.Interval;

        return output;
    }

    float pulse()
    {
        if (duty < 0.0f) duty = 0.0f;
        if (duty > 1.0f) duty = 1.0f;
        if (phase < duty) output = -1.0f;
        if (phase > duty) output = 1.0f;

        if (phase >= 1.0f) phase -= 1.0f;
        phase += frequency * AudioProperties.Interval;

        return output;
    }

    float triangle()
    {
        if (phase <= 0.5f) output = (phase - 0.25f) * 4.0f;
        else output = ((1.0f - phase) - 0.25f) * 4.0f;

        if (phase >= 1.0f) phase -= 1.0f;
        phase += frequency * AudioProperties.Interval;

        return output;
    }

    float noise()
    {
        output = 2.0f * (float)rand.NextDouble() - 1.0f;

        return output;
    }
}
