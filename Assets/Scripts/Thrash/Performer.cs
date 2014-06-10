using UnityEngine;
using System.Collections;

public class Performer : MonoBehaviour {

    Instrument instrument;

    Note[] score;

    bool change;
    Note[] currentBar;
    int[] notes = { 36, 38, 40, 47 };

    void Start()
    {
        instrument = FindObjectOfType<SynthIntstrument>();

        //score = new Note[MainClock.BarLength]; 
        //for (int i = 0; i < score.Length; ++i)
        //{
        //    score[i] = new Note(Random.Range(36, 48), Random.Range(0.75f, 1.0f));
        //}
        currentBar = new Note[MainClock.BarLength];

        for (int i = 0; i < currentBar.Length; ++i)
        {
            currentBar[i] = new Note((i % 4 == 0 && Random.Range(0.0f, 1.0f) > 0.25f) ? notes[Random.Range(0, notes.Length - 1)] : 0, Random.Range(0.75f, 1.0f));
        }
    }

    void Update()
    {
        if (change)
        {
            change = false;
            for (int i = 0; i < currentBar.Length; ++i)
            {
                currentBar[i] = new Note((i % 4 == 0 && Random.Range(0.0f, 1.0f) > 0.3f) ? notes[Random.Range(0, notes.Length - 1)] : 0, Random.Range(0.6f, 1.0f));
            }
        }
    }

    void OnEnable()
    {
        AudioEventManager.OnNextPulse += OnNextPulse;
        AudioEventManager.OnNextBar += OnNextBar;
    }

    void OnDisable()
    {
        AudioEventManager.OnNextPulse -= OnNextPulse;
        AudioEventManager.OnNextBar -= OnNextBar;
    }

    void OnNextBar(int bar)
    {
        //for (int i = 0; i < score.Length; ++i)
        //{
        //   // score[i] = new Note(Random.Range(36, 48), Random.Range(0.75f, 1.0f));
        //}

        change = true;
    }

    void OnNextPulse(int pulse)
    {
       // if (pulse % 2 == 0)
        {
            if (currentBar[pulse % currentBar.Length].Index > 0)
                instrument.AddNote(currentBar[pulse % currentBar.Length]);
            else
                instrument.RemoveAllNotes();
        }
    }
}
