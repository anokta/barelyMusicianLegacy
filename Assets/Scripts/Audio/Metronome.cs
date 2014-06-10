using UnityEngine;
using System.Collections;

public class Metronome : MonoBehaviour {

    PercussiveInstrument instrument;

	// Use this for initialization
	void Start () {
        instrument = GetComponent<PercussiveInstrument>();
        
        AudioEventManager.OnNextBar += OnNextBar;
        AudioEventManager.OnNextBeat += OnNextBeat;
	}
	
    void OnNextBar(int bar)
    {
        instrument.AddNote(new Note(instrument.rootIndex, 1.0f));
    }

    void OnNextBeat(int beat)
    {
        instrument.AddNote(new Note(instrument.rootIndex+1, 1.0f));
    }
}
