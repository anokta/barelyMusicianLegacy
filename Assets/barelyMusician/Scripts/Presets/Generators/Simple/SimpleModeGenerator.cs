using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class SimpleModeGenerator : ModeGenerator
    {
        public override void GenerateScale(float energy, float stress)
        {
            if (stress < -0.5f)
            {
                setScale(MusicalScale.MAJOR, MusicalMode.IONIAN);
            }
            else if (stress > 0.5f)
            {
                setScale(MusicalScale.HARMONIC_MINOR, MusicalMode.IONIAN);
            }
            else
            {
                setScale(MusicalScale.NATURAL_MINOR, MusicalMode.IONIAN);
            }
        }
    }
}