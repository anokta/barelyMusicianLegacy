using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    // TODO Find a better solution!
    [Serializable]
    public class InstrumentMeta : ScriptableObject
    {
        public int Type;
        public bool Active = true;
        public float Volume = 0.0f;
        public float Attack = 0.0f, Decay = 0.25f, Sustain = 1.0f, Release = 0.25f;
        
        public int RootIndex = 0;
        public bool Sustained = false;
        
        public int VoiceCount = 16;
        
        public OscillatorType OscType = OscillatorType.SINE;
        public AudioClip Sample;
        public AudioClip[] Samples;

        public List<int> effects;
    }
}