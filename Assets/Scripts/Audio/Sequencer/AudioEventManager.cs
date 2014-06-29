using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public static class AudioEventManager
    {

        public delegate void AudioEvent(int count);
        public delegate void AudioSequencerEvent();

        // Audio Events
        public static event AudioEvent OnNextBar, OnNextBeat, OnNextPulse;
        public static event AudioSequencerEvent OnPlay, OnPause, OnStop;


        public static void TriggerOnNextBar(int bar)
        {
            if (OnNextBar != null)
            {
                OnNextBar(bar);
            }

            //Debug.Log(Sequencer.barCount);
        }

        public static void TriggerOnNextBeat(int beat)
        {
            if (OnNextBeat != null)
            {
                OnNextBeat(beat);
            }

            //Debug.Log(Sequencer.barCount + "/" + Sequencer.beatCount);
        }

        public static void TriggerOnNextPulse(int pulse)
        {
            if (OnNextPulse != null)
            {
                OnNextPulse(pulse);
            }
        }

        public static void TriggerOnPlay()
        {
            if (OnPlay != null)
            {
                OnPlay();
            }
        }

        public static void TriggerOnPause()
        {
            if (OnPause != null)
            {
                OnPause();
            }
        }
        public static void TriggerOnStop()
        {
            if (OnStop != null)
            {
                OnStop();
            }
        }
    }
}
