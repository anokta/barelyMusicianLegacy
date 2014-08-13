using UnityEngine;
using System;
using System.Collections;

namespace BarelyAPI
{
    [Serializable]
    public class InstrumentMeta : ScriptableObject
    {
        public int type;
        public float volume = 0.0f;

        public float attack = 0.0f, decay = 0.25f, sustain = 1.0f, release = 0.25f;
        public bool sustained = false;
        public int voiceCount = 16;
        public OscillatorType oscType = OscillatorType.SINE;
        public AudioClip sample;
        public AudioClip[] samples;
    }
}