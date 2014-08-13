using UnityEngine;
using System.Collections;
using BarelyAPI;

public class KeyboardController : MonoBehaviour
{
    public int fundamentalIndex = (int)NoteIndex.C4;
    public OscillatorType oscType;
    public float attack, decay, sustain, release;

    KeyCode[] keys = 
    { 
        KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.E, KeyCode.D, KeyCode.F, KeyCode.T, KeyCode.G, KeyCode.Y, KeyCode.H, KeyCode.U, KeyCode.J, KeyCode.K, KeyCode.O, KeyCode.L 
    };

    Instrument instrument;
    AudioSource audioSource;

    void Awake()
    {
        InstrumentMeta meta = ScriptableObject.CreateInstance<InstrumentMeta>();
        meta.RootIndex = fundamentalIndex;
        meta.Type = 2;
        meta.OscType = oscType;
        meta.Attack = attack;
        meta.Decay = decay;
        meta.Sustain = sustain;
        meta.Release = release;
        instrument = InstrumentFactory.CreateInstrument(meta);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.hideFlags = HideFlags.HideInInspector;
        audioSource.panLevel = 0.0f;
        audioSource.Play();
    }

    void Update()
    {
        // octave up-down
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fundamentalIndex = Mathf.Max(-36, fundamentalIndex - 12);
            instrument.StopAllNotes();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            fundamentalIndex = Mathf.Min(36, fundamentalIndex + 12);
            instrument.StopAllNotes();
        }

        // keys
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyUp(keys[i]))
            {
                instrument.PlayNote(new Note(fundamentalIndex + i, 0.0f));
            }
            else if (Input.GetKeyDown(keys[i]))
            {
                instrument.PlayNote(new Note(fundamentalIndex + i, 1.0f));
            }
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = instrument.ProcessNext();

            // If stereo, copy the mono data to each channel
            if (channels == 2) data[i + 1] = data[i];
        }
    }
}