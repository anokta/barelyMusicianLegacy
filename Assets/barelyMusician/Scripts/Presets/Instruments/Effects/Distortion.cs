using UnityEngine;
using System.Collections;

public class Distortion : AudioEffect
{

    // Distortion level
    private float level;
    public float Level
    {
        get { return level; }
        set { level = value; }
    }

    public Distortion(float distortionLevel)
    {
        level = distortionLevel;
    }

    public float Process(float sample)
    {
        return sample * level;
    }
}