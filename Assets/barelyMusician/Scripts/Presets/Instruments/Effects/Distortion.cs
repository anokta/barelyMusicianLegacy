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

    public void Process(ref float[] data)
    {
        for (int i = 0; i < data.Length; ++i)
        {
            data[i] *= level;
        }
    }
}