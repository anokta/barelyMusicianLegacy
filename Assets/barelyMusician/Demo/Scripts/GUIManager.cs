using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GUIManager : MonoBehaviour
{

    public GUIText timeSignutare;
    //private string timeSigText;

    public Musician musician;

    // Use this for initialization
    void Start()
    {
        //AudioEventManager.OnNextBeat += OnNextBeat;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnGUI()
    {
        //timeSignutare.text = timeSigText;
        GUILayout.BeginArea(new Rect(Screen.width * 0.25f, Screen.height * 0.2f, Screen.width * 0.5f, Screen.height * 0.75f));

        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();

        //GUILayout.BeginHorizontal();
        //GUILayout.Box("BPM: " + Sequencer.Tempo.ToString());
        //Sequencer.Tempo = (int)GUILayout.HorizontalSlider(Sequencer.Tempo, 72, 220);
        //GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Play"))
        {
            musician.Play();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Pause"))
        {
            musician.Pause();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Stop"))
        {
            musician.Stop();
        }

        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("EXCITING"))
        {
            musician.SetMood(Mood.EXCITING, 0.5f);
        }
        if (GUILayout.Button("HAPPY"))
        {
            musician.SetMood(Mood.HAPPY, 0.5f);
        }
        if (GUILayout.Button("TENDER"))
        {
            musician.SetMood(Mood.TENDER, 0.5f);
        }
        if (GUILayout.Button("DEPRESSED"))
        {
            musician.SetMood(Mood.DEPRESSED, 0.5f);
        }
        if (GUILayout.Button("SAD"))
        {
            musician.SetMood(Mood.SAD, 0.5f);
        }
        if (GUILayout.Button("ANGRY"))
        {
            musician.SetMood(Mood.ANGRY, 0.5f);
        }
        GUILayout.EndHorizontal();

        //GUILayout.BeginVertical();
        GUILayout.Box("Energy");
        musician.Energy = GUILayout.HorizontalSlider(musician.Energy, 0.0f, 1.0f);
        GUILayout.Box("Stress");
        musician.Stress = GUILayout.HorizontalSlider(musician.Stress, 0.0f, 1.0f);
        //GUILayout.EndVertical();


        //GUILayout.BeginVertical();
        //ensemble.PrintValues();
        //GUILayout.EndVertical();

        //GUILayout.EndHorizontal();
        //

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();

        GUILayout.EndArea();
    }

    void OnNextBeat(int beat)
    {
        //timeSigText = beat.ToString() + " / " + Sequencer.BeatCount;
    }
}