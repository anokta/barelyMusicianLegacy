using UnityEngine;

namespace BarelyMusician
{
    public class Sampler : UGen
    {
        // Sample raw data
        float[] sampleData;
        float samplingRatio;

        float rootFrequency;
        public float RootFrequency
        {
            get { return rootFrequency; }
            set { rootFrequency = value; }
        }

        // Should loop?
        bool loop;
        public bool Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        public Sampler(AudioClip sample, bool loop = false, float rootFrequency = 440.0f)
        {
            // Supports MONO samples only (for now)
            sampleData = new float[sample.samples];
            sample.GetData(sampleData, 0);

            RootFrequency = rootFrequency;
            samplingRatio = sample.frequency / AudioProperties.SampleRate;

            Loop = loop;

            Reset();
        }

        public override float Next()
        {
            if (phase >= sampleData.Length)
            {
                if (loop) phase -= sampleData.Length;
                else return 0.0f;
            }

            output = sampleData[(int)(phase)];

            phase += (frequency / rootFrequency) * samplingRatio;

            return output;
        }
    }
}