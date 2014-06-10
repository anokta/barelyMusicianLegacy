using UnityEngine;
using System.Collections;

public class PreciseSequenceTest : MonoBehaviour {

    public AudioClip sample;
    Sampler sampler;

    bool trigger;


	void Start () {
        sampler = new Sampler(sample);
        sampler.Frequency = sampler.RootFrequency;
	
        AudioEventManager.OnNextBeat += OnNextBeat;
	}
	

    void OnNextBeat(int beat)
    {
        sampler.Reset();
        trigger = true;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (trigger)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                data[i] = sampler.Next();

                // If stereo, copy the mono data to each channel
                if (channels == 2) data[i + 1] = data[i];
            }
        }
    }
}
