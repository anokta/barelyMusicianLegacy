using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GUIManager : MonoBehaviour
{
    public GUISkin guiSkin;

    Musician musician;
    Recorder recorder;

    void Start()
    {
        musician = FindObjectOfType<Musician>();
        recorder = FindObjectOfType<Recorder>();
    }

    void OnGUI()
    {
        GUI.skin = guiSkin;

        GUILayout.BeginArea(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.75f));

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Play"))
        {
            musician.Play();
        }
        if (GUILayout.Button("Pause"))
        {
            musician.Pause();
        }
        if (GUILayout.Button("Stop"))
        {
            musician.Stop();
        }
        bool recording  = GUILayout.Toggle(recorder.IsRecording, "Record");
        if (recording != recorder.IsRecording)
        {
            if (recording) recorder.StartRecord();
            else recorder.StopRecord();
        }
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Exciting", guiSkin.GetStyle("Positive")))
        {
            musician.SetMood(Mood.Exciting, 0.5f);
        }
        if (GUILayout.Button("Happy", guiSkin.GetStyle("Positive")))
        {
            musician.SetMood(Mood.Happy, 0.5f);
        }
        if (GUILayout.Button("Tender", guiSkin.GetStyle("Positive")))
        {
            musician.SetMood(Mood.Tender, 0.5f);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Neutral", guiSkin.GetStyle("Neutral")))
        {
            musician.SetMood(Mood.Neutral, 0.5f);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Depressed", guiSkin.GetStyle("Negative")))
        {
            musician.SetMood(Mood.Depressed, 0.5f);
        }
        if (GUILayout.Button("Sad", guiSkin.GetStyle("Negative")))
        {
            musician.SetMood(Mood.Sad, 0.5f);
        }
        if (GUILayout.Button("Angry", guiSkin.GetStyle("Negative")))
        {
            musician.SetMood(Mood.Angry, 0.5f);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Box("Energy");
        float energy = GUILayout.HorizontalSlider(musician.Energy, 0.0f, 1.0f);
        if (energy != musician.Energy)
            musician.SetEnergy(energy);
        GUILayout.Box("Stress");
        float stress = GUILayout.HorizontalSlider(musician.Stress, 0.0f, 1.0f);
        if (stress != musician.Stress)
            musician.SetStress(stress);
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();

        GUILayout.EndArea();
    }
}