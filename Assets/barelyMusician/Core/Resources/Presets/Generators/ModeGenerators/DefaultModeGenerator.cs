using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class DefaultModeGenerator : ModeGenerator
    {
        public override void GenerateScale(float stress)
        {
            if (stress < 0.25f)
            {
                setScale(MusicalScale.MAJOR, MusicalMode.IONIAN);
            }
            else if (stress < 0.5f)
            {
                setScale(MusicalScale.NATURAL_MINOR, MusicalMode.IONIAN);
            }
            else
            {
                setScale(MusicalScale.HARMONIC_MINOR, MusicalMode.IONIAN);
            }
        }
    }
}