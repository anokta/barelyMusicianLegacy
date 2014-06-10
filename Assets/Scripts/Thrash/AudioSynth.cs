using UnityEngine;
using System.Collections;

public class AudioSynth : MonoBehaviour
{
    // un-optimized version
    public float fundamental = 261.63f;
    public float gain = 0.05f;

    //private float frequency = 440;
    private float increment;
    private float phase;
    //private float sampling_frequency;

    private bool change;

    private KeyCode[] keys = { KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.E, KeyCode.D, KeyCode.F, KeyCode.T, KeyCode.G, KeyCode.Y, KeyCode.H, KeyCode.U, KeyCode.J, KeyCode.K, KeyCode.O, KeyCode.L };

    int[] currentBar;
    int[] notes = { 60, 62, 64, 71 };

    //Envelope envelope;
    Audible voice;
    //Sonic sonic;

    public AudioClip sample;

    void Awake()
    {
        //sampling_frequency = AudioSettings.outputSampleRate;

        currentBar = new int[16];

        for (int i = 0; i < currentBar.Length; ++i)
        {
            currentBar[i] = (i % 2 == 0 && Random.Range(0.0f, 1.0f) > 0.25f) ? notes[Random.Range(0, notes.Length - 1)] : 0;
        }

        AudioEventManager.OnNextBar += OnNextBar;
        AudioEventManager.OnNextPulse += OnNextPulse;

        //osc = new Oscillator(Oscillator.OSCType.SQUARE);
        //osc.PulseDuty = 0.25f;

        //envelope = new Envelope(0.2f, 0.5f, 0.75f, 1.0f);
    }

    void Start()
    {
        voice = new Impulse(new Sampler(sample), 2.0f);//new Voice(new Sampler(sample), new Envelope(0.2f, 0.5f, 0.75f, 1.0f));//new Voice(new Oscillator(Oscillator.OSCType.SQUARE), new Envelope(0.2f, 0.5f, 0.75f, 1.0f), 1.0f);
        //sonic = new Sampler(sample);
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
                        voice.Pitch = fundamental * Mathf.Pow(1.0594f, j);
                        anykey = true;
                    }
                }

                if (!anykey)
                {
                    voice.NoteOff();
                }
            }
            else if (Input.GetKeyDown(keys[i]))
            {
                voice.Pitch = fundamental * Mathf.Pow(1.0594f, i);
                voice.NoteOn();
                    
            }
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
            for (var i = 0; i < data.Length; i = i + channels)
            {
                data[i] = voice.ProcessNext();
                // if we have stereo, we copy the mono data to each channel
                if (channels == 2) data[i + 1] = data[i];
            }
    }

    void OnNextPulse(int clock)
    {
        if (currentBar[clock] > 0)
        {
            voice.Pitch = fundamental * Mathf.Pow(1.0594f, currentBar[clock] - 60);
            voice.NoteOn();
        }
        else
        {
           //voice.NoteOff();
        }
    }

    void OnNextBar(int bar)
    {
        change = true;
    }
}

