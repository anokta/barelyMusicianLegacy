using UnityEngine;
using System.Collections;

public class MainClock : MonoBehaviour
{
    public int BPM = 120;

    public int BEATS = 4;
    public int NOTE_TYPE = 4;
    public int CLOCK_FREQ = 16;

    private double phasor;
    private double beatInterval;

    public static int barCount;
    public static int beatCount;
    public static int pulseCount;

    static MainClock _instance;

    void Awake()
    {
        _instance = this;

        Reset();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            phasor++;

            beatInterval = 240.0f * AudioProperties.SampleRate / CLOCK_FREQ / BPM;
            if (phasor >= beatInterval)
            {
                pulseCount = (pulseCount + 1) % CLOCK_FREQ;

                if (pulseCount % (CLOCK_FREQ / NOTE_TYPE) == 0)
                {
                    beatCount = beatCount % BEATS + 1;
                    if (beatCount == 1)
                    {
                        barCount = barCount + 1;
                        AudioEventManager.TriggerOnNextBar(barCount);
                    }

                    AudioEventManager.TriggerOnNextBeat(beatCount);
                }

                AudioEventManager.TriggerOnNextPulse(pulseCount);

                phasor %= beatInterval;
            }
        }
    }

    void Reset()
    {
        beatInterval = 240.0f * AudioProperties.SampleRate / CLOCK_FREQ / BPM;
        phasor = beatInterval;

        barCount = 0;
        beatCount = 0;
        pulseCount = -1;
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

    public static int BarLength
    {
        get { return _instance.BEATS * (_instance.CLOCK_FREQ / _instance.NOTE_TYPE); }
    }
}
