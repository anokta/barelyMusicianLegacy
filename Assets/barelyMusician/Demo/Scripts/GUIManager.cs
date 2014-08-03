using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GUIManager : MonoBehaviour
{

    public GUIText timeSignutare;
    //private string timeSigText;

    public Producer producer;

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
            producer.Play();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Pause"))
        {
            producer.Pause();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Stop"))
        {
            producer.Stop();
        }

        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("EXCITING"))
        {
            producer.SetMood(Mood.EXCITING, 0.5f);
        }
        if (GUILayout.Button("HAPPY"))
        {
            producer.SetMood(Mood.HAPPY, 0.5f);
        }
        if (GUILayout.Button("TENDER"))
        {
            producer.SetMood(Mood.TENDER, 0.5f);
        }
        if (GUILayout.Button("DEPRESSED"))
        {
            producer.SetMood(Mood.DEPRESSED, 0.5f);
        }
        if (GUILayout.Button("SAD"))
        {
            producer.SetMood(Mood.SAD, 0.5f);
        }
        if (GUILayout.Button("ANGRY"))
        {
            producer.SetMood(Mood.ANGRY, 0.5f);
        }
        GUILayout.EndHorizontal();

        //GUILayout.BeginVertical();
        GUILayout.Box("Energy");
        producer.Energy = GUILayout.HorizontalSlider(producer.Energy, 0.0f, 1.0f);
        GUILayout.Box("Stress");
        producer.Stress = GUILayout.HorizontalSlider(producer.Stress, 0.0f, 1.0f);
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