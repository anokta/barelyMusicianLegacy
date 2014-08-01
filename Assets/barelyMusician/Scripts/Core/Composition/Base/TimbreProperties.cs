﻿using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class TimbreProperties
    {
        // Note onset
        float noteOnsetMult;
        public float NoteOnsetMultiplier
        {
            get { return noteOnsetMult; }
            set { noteOnsetMult = 0.25f + 1.75f * value; }
        }

        // Timbre brightness
        float brightness;
        public float Brightness
        {
            get { return brightness; }
            set { brightness = value; }
        }

        float tense;
        public float Tense
        {
            get { return tense; }
            set { tense = value; }
        }

        public void Set(float energy, float stress)
        {
            NoteOnsetMultiplier = 1.0f - energy;

            Brightness = 0.75f * energy + 0.25f * (1.0f - stress);
            Tense = 0.75f * stress + 0.25f * energy;
        }
    }
}