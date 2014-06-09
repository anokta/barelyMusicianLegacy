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
        instrument.AddNote(instrument.rootPitch);
    }

    void OnNextBeat(int beat)
    {
        instrument.AddNote(instrument.rootPitch * 1.0594f);
    }
}
