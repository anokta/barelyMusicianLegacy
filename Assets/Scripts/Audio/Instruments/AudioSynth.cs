using UnityEngine;
using System.Collections;  // Needed for Math

public class AudioSynth : MonoBehaviour
{
    // un-optimized version
    public float fundamental = 261.63f;
    public float gain = 0.05f;

    private float frequency = 440;
    private float increment;
    private float phase;
    private float sampling_frequency;

    private bool change;

    private KeyCode[] keys = { KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.E, KeyCode.D, KeyCode.F, KeyCode.T, KeyCode.G, KeyCode.Y, KeyCode.H, KeyCode.U, KeyCode.J, KeyCode.K, KeyCode.O, KeyCode.L };

    int[] currentBar;
    int[] notes = { 60, 62, 64, 71 };

    Envelope envelope;

    void Awake()
    {
        sampling_frequency = AudioSettings.outputSampleRate;

        currentBar = new int[16];

        for (int i = 0; i < currentBar.Length; ++i)
        {
            currentBar[i] = (i % 2 == 0 && Random.Range(0.0f, 1.0f) > 0.25f) ? notes[Random.Range(0, notes.Length-1)] : 0;
        }

        AudioEventManager.OnNextBar += OnNextBar;
        AudioEventManager.OnNextTrig += OnNextTrig;

        envelope = new Envelope(0.5f, 1.0f, 0.75f, 1.0f);
    }

    void Update()
    {
        if (change)
        {
            change = false;
            for (int i = 0; i < currentBar.Length; ++i)
            {
                currentBar[i] = (i % 2 == 0 && Random.Range(0.0f, 1.0f) > 0.25f) ? notes[Random.Range(0, notes.Length - 1)] : 0;
            }
        }
    }

    void OnGUI()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyUp(keys[i]))
            {
                bool anykey = false;
                for (int j = 0; j < keys.Length; j++)
                {
                    if (Input.GetKey(keys[j]))
                    {
                        frequency = fundamental * Mathf.Pow(1.0594f, j);
                        anykey = true; 
                    }
                }

                if (!anykey)
                {
                    envelope.NoteOn = false;
                }
            }
            else if (Input.GetKeyDown(keys[i]))
            {
                frequency = fundamental * Mathf.Pow(1.0594f, i);
                envelope.NoteOn = true;
            }
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        
            // update increment in case frequency has changed
            increment = frequency * 2 * Mathf.PI / sampling_frequency;
            for (var i = 0; i < data.Length; i = i + channels)
            {
                envelope.OnNextBlock(1.0f / 44100);

                phase = phase + increment;
                // this is where we copy audio data to make them “available” to Unity
                data[i] = envelope.Multiplier * (float)(gain * (phase / (2*Mathf.PI)));
                // if we have stereo, we copy the mono data to each channel
                if (channels == 2) data[i + 1] = data[i];
                if (phase > 2 * Mathf.PI) phase = 0;
            }
    }

    void OnNextTrig(int clock)
    {
        if (currentBar[clock] > 0)
        {
            frequency = fundamental * Mathf.Pow(1.0594f, currentBar[clock] - 60);
            envelope.NoteOn = true;
        }
        else
        {
            envelope.NoteOn = false;
        }
    }

    void OnNextBar(int bar)
    {
        change = true;
    }
}

