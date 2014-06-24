using UnityEngine;
using System.Collections;

namespace BarelyMusician
{
    public class MainClock : MonoBehaviour
    {
        public int BPM = 120;

        public int BEATS = 4;
        public int NOTE_TYPE = 4;
        public int CLOCK_FREQ = 16;

        private double phasor;
        private double clockInterval;

        public static int currentBar;
        public static int currentBeat;
        public static int currentPulse;

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
                    currentPulse = currentPulse % BarLength + 1;

                    if (currentPulse % BeatLength == 1)
                    {
                        currentBeat = currentBeat % BEATS + 1;
                        if (currentBeat == 1)
                        {
                            currentBar = currentBar + 1;
                            AudioEventManager.TriggerOnNextBar(currentBar);
                        }

                        AudioEventManager.TriggerOnNextBeat(currentBeat);
                    }

                    AudioEventManager.TriggerOnNextPulse(currentPulse);

                    phasor %= clockInterval;
                }
            }
        }

        void Reset()
        {
            phasor = clockInterval;

            currentBar = 0;
            currentBeat = 0;
            currentPulse = 0;
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

        public static int BeatCount
        {
            get { return _instance.BEATS; }
        }
    }
}