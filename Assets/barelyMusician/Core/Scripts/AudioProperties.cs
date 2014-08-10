using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public static class AudioProperties
    {
        public static float MIN_VOLUME_DB = -60.0f;
        public static float MAX_VOLUME_DB = 6.0f;

        public static int SAMPLE_RATE = AudioSettings.outputSampleRate = 44100;
        public static float INTERVAL = 1.0f / SAMPLE_RATE;
    }
}