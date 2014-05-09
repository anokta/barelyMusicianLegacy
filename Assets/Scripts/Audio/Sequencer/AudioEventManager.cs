using UnityEngine;
using System.Collections;

public static class AudioEventManager {

    public delegate void AudioEvent(int count);

    // Audio Events
    public static event AudioEvent OnNextBar, OnNextBeat, OnNextTrig;


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

    public static void TriggerOnNextTrig(int clock)
    {
        if (OnNextTrig != null)
        {
            OnNextTrig(clock);
        }
    }
}
