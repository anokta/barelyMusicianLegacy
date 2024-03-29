﻿// ----------------------------------------------------------------------
//   Adaptive music composition engine implementation for interactive systems.
//
//     Copyright 2014 Alper Gungormusler. All rights reserved.
//
// ------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GUIManager : MonoBehaviour
{
    public GUISkin guiSkin;
    public float smoothness;

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

#if UNITY_WEBPLAYER
        GUI.enabled = false;
#endif
        bool recording  = GUILayout.Toggle(recorder.IsRecording, "Record");
        if (recording != recorder.IsRecording)
        {
            if (recording) recorder.StartRecord();
            else recorder.StopRecord();
        }
#if UNITY_WEBPLAYER
        GUI.enabled = true;
#endif

        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Exciting", guiSkin.GetStyle("Positive")))
        {
            musician.SetMood(Mood.Exciting, smoothness);
        }
        if (GUILayout.Button("Happy", guiSkin.GetStyle("Positive")))
        {
            musician.SetMood(Mood.Happy, smoothness);
        }
        if (GUILayout.Button("Tender", guiSkin.GetStyle("Positive")))
        {
            musician.SetMood(Mood.Tender, smoothness);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Neutral", guiSkin.GetStyle("Neutral")))
        {
            musician.SetMood(Mood.Neutral, smoothness);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Depressed", guiSkin.GetStyle("Negative")))
        {
            musician.SetMood(Mood.Depressed, smoothness);
        }
        if (GUILayout.Button("Sad", guiSkin.GetStyle("Negative")))
        {
            musician.SetMood(Mood.Sad, smoothness);
        }
        if (GUILayout.Button("Angry", guiSkin.GetStyle("Negative")))
        {
            musician.SetMood(Mood.Angry, smoothness);
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