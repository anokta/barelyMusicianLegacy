using UnityEngine;
using System.Collections;

public class Instrument : MonoBehaviour {

    protected AudioSource sample;

    protected bool noteOn;
    protected bool trigger;

    void Awake () 
    {
        sample = audio;

        noteOn = false;
        trigger = false;
	}
	
	void OnGUI() 
    {
        if (trigger || noteOn)
        {
            trigger = false;

            if (!sample.isPlaying)
            {
                sample.Play();
            }
        }
	}

    public void Trigger()
    {
        trigger = true;
    }

    public void NoteOn()
    {
        noteOn = true;
    }

    public void NoteOff()
    {
        noteOn = false;
    }
}
