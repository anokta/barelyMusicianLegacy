using UnityEngine;
using System.Collections;

public class Instrument : MonoBehaviour {

    public AudioClip sample;

    protected float[] sampleData;
    protected int sampleOffset;

    protected bool noteOn;
    protected bool trigger;

    void Awake () 
    {
        sampleData = new float[sample.samples];
        sample.GetData(sampleData, 0);
        sampleOffset = -1;

        noteOn = false;
        trigger = false;
	}
	
    public void Trigger()
    {
        trigger = true;
        sampleOffset = 0;
    }

    public void NoteOn()
    {
        noteOn = true;
    }

    public void NoteOff()
    {
        noteOn = false;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (sampleOffset >= 0)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                if (sampleOffset < sampleData.Length)
                {
                    data[i] = sampleData[sampleOffset];
                    if (channels == 2) data[i + 1] = sampleData[sampleOffset];
                    sampleOffset++;
                }
                else
                {
                    sampleOffset += -1;
                    break;
                }
            }
        }
    }
}
