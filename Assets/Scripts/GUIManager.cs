using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GUIManager : MonoBehaviour
{

    public GUIText timeSignutare;
    private string timeSigText;

    // Use this for initialization
    void Start()
    {
        AudioEventManager.OnNextBeat += OnNextBeat;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnGUI()
    {
        timeSignutare.text = timeSigText;
        GUILayout.BeginArea(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.5f));

        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Box("BPM: " + MainClock.Tempo.ToString());
        MainClock.Tempo = (int)GUILayout.HorizontalSlider(MainClock.Tempo, 72, 220);
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Play"))
        {
            MainClock.Play();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Pause"))
        {
            MainClock.Pause();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Stop"))
        {
            MainClock.Stop();
        }

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();

        GUILayout.EndArea();
    }

    void OnNextBeat(int beat)
    {
        timeSigText = beat.ToString() + " / " + MainClock.BeatCount;
    }
}
