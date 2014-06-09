using UnityEngine;
using System.Collections;  // Needed for Math

public class AudioSynth : MonoBehaviour
{
    // un-optimized version
    public float fundamental = 261.63f;
    public float gain = 0.05f;

    //private float frequency = 440;
    private float increment;
    private float phase;
    private float sampling_frequency;

    private bool change;

    private KeyCode[] keys = { KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.E, KeyCode.D, KeyCode.F, KeyCode.T, KeyCode.G, KeyCode.Y, KeyCode.H, KeyCode.U, KeyCode.J, KeyCode.K, KeyCode.O, KeyCode.L };

    int[] currentBar;
    int[] notes = { 60, 62, 64, 71 };

    Envelope envelope;
    Oscillator osc;

    void Awake()
    {
        sampling_frequency = AudioSettings.outputSampleRate;

        currentBar = new int[16];

        for (int i = 0; i < currentBar.Length; ++i)
        {
            currentBar[i] = (i % 2 == 0 && Random.Range(0.0f, 1.0f) > 0.25f) ? notes[Random.Range(0, notes.Length - 1)] : 0;
        }

        AudioEventManager.OnNextBar += OnNextBar;
        AudioEventManager.OnNextTrig += OnNextTrig;

        osc = new Oscillator(Oscillator.OSCType.SQUARE);
        osc.PulseDuty = 0.25f;
        osc.Frequency = 440.0f;
        envelope = new Envelope(0.2f, 0.5f, 0.75f, 1.0f);
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
                        osc.Frequency = fundamental * Mathf.Pow(1.0594f, j);
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
                osc.Frequency = fundamental * Mathf.Pow(1.0594f, i);
                envelope.NoteOn = true;
            }
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (var i = 0; i < data.Length; i = i + channels)
        {
            envelope.ProcessNext();

            data[i] = envelope.Multiplier * gain * osc.Next();
            // if we have stereo, we copy the mono data to each channel
            if (channels == 2) data[i + 1] = data[i];

        }
    }

    void OnNextTrig(int clock)
    {
        if (currentBar[clock] > 0)
        {
            osc.Frequency = fundamental * Mathf.Pow(1.0594f, currentBar[clock] - 60);
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

