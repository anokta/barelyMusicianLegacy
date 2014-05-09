using UnityEngine;
using System.Collections;

public class MainClock : MonoBehaviour
{
    public int BPM = 120;

    public int BEATS = 4;
    public int NOTE_TYPE = 4;
    public int CLOCK_FREQ = 16;

    private int sampleRate;
    private int bufferSize;

    private double phasor;
    private double beatInterval;

    public static int barCount;
    public static int beatCount;
    public static int clockCount;

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
        phasor += bufferSize;

        beatInterval = 240.0f * sampleRate / CLOCK_FREQ / BPM;
        if (phasor >= beatInterval)
        {
            clockCount = (clockCount + 1) % CLOCK_FREQ;

            if (clockCount % (CLOCK_FREQ / NOTE_TYPE) == 0)
            {
                beatCount = beatCount % BEATS + 1;
                if (beatCount == 1)
                {
                    barCount = barCount + 1;
                    AudioEventManager.TriggerOnNextBar(barCount);
                }

                AudioEventManager.TriggerOnNextBeat(beatCount);
            }

            AudioEventManager.TriggerOnNextTrig(clockCount);

            phasor %= beatInterval;
        }
    }

    void Reset()
    {
        beatInterval = 240.0f * sampleRate / CLOCK_FREQ / BPM;
        phasor = beatInterval;

        barCount = 0;
        beatCount = 0;
        clockCount = -1;
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
