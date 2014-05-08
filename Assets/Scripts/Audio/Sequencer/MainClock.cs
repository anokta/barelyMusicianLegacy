using UnityEngine;
using System.Collections;

public class MainClock : MonoBehaviour
{
    public int BPM = 120;

    public int BEATS = 4;
    public int NOTE_TYPE = 4;

    private int sampleRate;
    private int bufferSize;

    private double phaser;
    private double beatInterval;

    public static int barCount;
    public static int beatCount;

    static MainClock _instance;

    void Awake()
    {
        _instance = this; 

        // ISSUE: Output sample rate does not correspond to the DSP sample rate for some reason by default.
        sampleRate = AudioSettings.outputSampleRate = 44100;
        int numBuffers; 
        AudioSettings.GetDSPBufferSize(out bufferSize, out numBuffers);

        Reset();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        phaser += bufferSize;

        beatInterval = 240.0f * sampleRate / NOTE_TYPE / BPM;
        if (phaser >= beatInterval)
        {
            beatCount = beatCount % BEATS + 1;
            if (beatCount == 1)
            {
                barCount = barCount + 1; 
                AudioEventManager.TriggerOnNextBar();
            }

            AudioEventManager.TriggerOnNextBeat();

            phaser %= beatInterval;
        }
    }

    void Reset()
    {
        beatInterval = 240.0f * sampleRate / NOTE_TYPE / BPM;
        phaser = beatInterval;

        barCount = 0;
        beatCount = 0;
    }

    public static void Play()
    {
        if (!_instance.audio.isPlaying)
        {
            _instance.audio.Play();
        }
    }

    public static void Pause()
    {
        if (_instance.audio.isPlaying)
        {
            _instance.audio.Pause();
        }
    }

    public static void Stop()
    {
        if (_instance.audio.isPlaying)
        {
            _instance.audio.Stop();
        }

        _instance.Reset();
    }

    public static bool IsPlaying()
    {
        return _instance.audio.isPlaying;
    }
}
