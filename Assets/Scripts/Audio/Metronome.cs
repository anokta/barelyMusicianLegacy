using UnityEngine;
using System.Collections;

public class Metronome : MonoBehaviour {

    public AudioSource high, low;
    bool highTrig, lowTrig;

	// Use this for initialization
	void Start () {
        AudioEventManager.OnNextBar += OnNextBar;
        AudioEventManager.OnNextBeat += OnNextBeat;
	}
	
	// Update is called once per frame
	void OnGUI () {
        if (highTrig)
        {
            highTrig = false;
            high.Play();
        }
        if (lowTrig)
        {
            lowTrig = false;
            low.Play();
        }
	}

    void OnNextBar()
    {
        highTrig = true;
    }

    void OnNextBeat()
    {
        if (!highTrig)
            lowTrig = true;
    }
}
