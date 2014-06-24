using UnityEngine;
using System.Collections;

namespace BarelyMusician
{
    public class AudioProperties : MonoBehaviour
    {
        public static int SampleRate;
        public static float Interval;

        public static int BufferSize;

        void Awake()
        {
            // ISSUE: Output sample rate does not correspond to the DSP sample rate for some reason by default.
            SampleRate = AudioSettings.outputSampleRate = 44100;
            Interval = 1.0f / SampleRate;

            int numBuffers;
            AudioSettings.GetDSPBufferSize(out BufferSize, out numBuffers);

            // For testing purposes (is it more precise?)
            //BufferSize = 512;
            //AudioSettings.SetDSPBufferSize(BufferSize, numBuffers);

            Note.Initialize();
        }
    }
}