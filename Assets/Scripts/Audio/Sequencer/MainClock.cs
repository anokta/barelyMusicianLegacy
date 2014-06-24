using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class MainClock : MonoBehaviour
    {
        public int BPM = 120;

        public int BEATS = 4;
        public int NOTE_TYPE = 4;
        public int CLOCK_FREQ = 16;

        private double phasor;
        private double clockInterval;

        public static int barCount;
        public static int beatCount;
        public static int pulseCount;

        static MainClock _instance;
        AudioSource audioSource;

        void Awake()
        {
            _instance = this;

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.hideFlags = HideFlags.HideInInspector;
            audioSource.Stop();

            clockInterval = 240.0f * AudioProperties.SampleRate / CLOCK_FREQ / BPM;

            Reset();
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                phasor++;

                clockInterval = 240.0f * AudioProperties.SampleRate / CLOCK_FREQ / BPM;
                if (phasor >= clockInterval)
                {
                    pulseCount = pulseCount % CLOCK_FREQ + 1;

                    if (pulseCount % (CLOCK_FREQ / NOTE_TYPE) == 1)
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

                    phasor %= clockInterval;
                }
            }
        }

        void Reset()
        {
            phasor = clockInterval;

            barCount = 0;
            beatCount = 0;
            pulseCount = 0;
        }

        public static void Play()
        {
            if (!_instance.audioSource.isPlaying)
            {
                _instance.audioSource.Play();
            }
        }

        public static void Pause()
        {
            if (_instance.audioSource.isPlaying)
            {
                _instance.audioSource.Pause();
            }
        }

        public static void Stop()
        {
            if (_instance.audioSource.isPlaying)
            {
                _instance.audioSource.Stop();
            }

            _instance.Reset();
        }

        public static bool IsPlaying()
        {
            return _instance.audioSource.isPlaying;
        }

        public static int BarLength
        {
            get { return _instance.BEATS * BeatLength; }
        }

        public static int BeatLength
        {
            get { return _instance.CLOCK_FREQ / _instance.NOTE_TYPE; }
        }
    }
}