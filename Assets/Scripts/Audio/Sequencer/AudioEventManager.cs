using UnityEngine;
using System.Collections;

public static class AudioEventManager {

    public delegate void AudioEvent();

    // Audio Events
    public static event AudioEvent OnNextBar, OnNextBeat;


    public static void TriggerOnNextBar()
    {
        if (OnNextBar != null)
        {
            OnNextBar();
        }

        //Debug.Log(Sequencer.barCount);
    }

    public static void TriggerOnNextBeat()
    {
        if (OnNextBeat != null)
        {
            OnNextBeat();
        }

        //Debug.Log(Sequencer.barCount + "/" + Sequencer.beatCount);
    }
}
