using UnityEngine;
using System.Collections;

public class Metronome : MonoBehaviour {

    public AudioClip high, low;
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
            audio.PlayOneShot(high);
        }
        if (lowTrig)
        {
            lowTrig = false;
            audio.PlayOneShot(low);
        }
	}

    void OnNextBar(int bar)
    {
        highTrig = true;
    }

    void OnNextBeat(int beat)
    {
        if (!highTrig)
            lowTrig = true;
    }
}
