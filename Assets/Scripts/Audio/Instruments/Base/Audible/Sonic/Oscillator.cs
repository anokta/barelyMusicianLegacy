using UnityEngine;
using System.Collections;
using System;

public class Oscillator : Sonic
{
    // Wave type
    public enum OSCType { SINE, COS, SAW, SQUARE, PULSE, TRIANGLE, NOISE };
    OSCType type;
    public OSCType Type
    {
        get 
        { 
            return type; 
        }
        set
        {
            type = value;

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
    }
    
    // Pulse duty (0. - 1.)
    float duty;
    public float PulseDuty
    {
        get { return duty; }
        set { duty = value; }
    }

    Func<float> oscFunc;

    static float TWO_PI = 2.0f * Mathf.PI;
    static System.Random rand = new System.Random();


    public Oscillator(OSCType type, float frequency = 440.0f, float duty = 0.0f)
    {
        Type = type;
        Frequency = frequency;
        PulseDuty = duty;
    }

    public override float Next()
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
