using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GUIManager : MonoBehaviour
{

    public GUIText timeSignutare;
    //private string timeSigText;

    public Ensemble ensemble;

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
            ensemble.Play();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Pause"))
        {
            ensemble.Pause();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Stop"))
        {
            ensemble.Stop();
        }

        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("EXCITING"))
        {
            ensemble.SetMood(Mood.EXCITING, 0.5f);
        }
        if (GUILayout.Button("HAPPY"))
        {
            ensemble.SetMood(Mood.HAPPY, 0.5f);
        }
        if (GUILayout.Button("TENDER"))
        {
            ensemble.SetMood(Mood.TENDER, 0.5f);
        }
        if (GUILayout.Button("DEPRESSED"))
        {
            ensemble.SetMood(Mood.DEPRESSED, 0.5f);
        }
        if (GUILayout.Button("SAD"))
        {
            ensemble.SetMood(Mood.SAD, 0.5f);
        }
        if (GUILayout.Button("ANGRY"))
        {
            ensemble.SetMood(Mood.ANGRY, 0.5f);
        }
        GUILayout.EndHorizontal();

        //GUILayout.BeginVertical();
        GUILayout.Box("Energy");
        ensemble.Energy = GUILayout.HorizontalSlider(ensemble.Energy, 0.0f, 1.0f);
        GUILayout.Box("Stress");
        ensemble.Stress = GUILayout.HorizontalSlider(ensemble.Stress, 0.0f, 1.0f);
        //GUILayout.EndVertical();


        GUILayout.BeginVertical();
        ensemble.PrintValues();
        GUILayout.EndVertical();

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
