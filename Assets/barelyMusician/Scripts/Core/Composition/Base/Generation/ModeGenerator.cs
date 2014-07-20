using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ModeGenerator
{
    public enum MusicalScale
    {
        MAJOR = 0, HARMONIC_MINOR = 1, NATURAL_MINOR = 2
    }
    public enum MusicalMode
    {
        IONIAN = 0, DORIAN = 1, PHRYGIAN = 2, LYDIAN = 3, MIXOLYDIAN = 4, AEOLIAN = 5, LOCRIAN = 6
    };

    static float[] majorScale = { 0, 2, 4, 5, 7, 9, 11 };
    static float[] harmonicMinorScale = { 0, 2, 3, 5, 7, 8, 11 };
    static float[] naturalMinorScale = { 0, 2, 3, 5, 7, 8, 10 };
    static Dictionary<MusicalScale, float[]> Scales = new Dictionary<MusicalScale, float[]>()
        {
            { MusicalScale.MAJOR, majorScale },
            { MusicalScale.HARMONIC_MINOR, harmonicMinorScale },
            { MusicalScale.NATURAL_MINOR, naturalMinorScale }
        };

    float[] currentScale;

    protected void setScale(MusicalScale scaleType, MusicalMode modeType = MusicalMode.IONIAN)
    {
        float[] scale = Scales[scaleType];
        int offset = (int)modeType;

        currentScale = new float[scale.Length];

        for (int i = 0; i < currentScale.Length; ++i)
        {
            currentScale[i] = scale[(i + offset) % scale.Length] + ((i + offset) / currentScale.Length) * 12;
        }
    }

    public float GetNoteOffset(int index)
    {
        int octaveOffset = Mathf.FloorToInt((float)index / currentScale.Length);
        int scaleOffset = index - octaveOffset * currentScale.Length;

        return currentScale[scaleOffset] + octaveOffset * 12;
    }

    public abstract void GenerateScale(float energy, float stress);
}