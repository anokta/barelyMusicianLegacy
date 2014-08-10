using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using BarelyAPI;

[CustomEditor(typeof(Musician))]
public class MusicianEditor : Editor
{
    Musician musician;

    void OnEnable()
    {
        musician = (Musician)target;
        musician.Init();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //foreach (Instrument instrument in instruments)
        //{
        //    EditorGUILayout.BeginHorizontal();
        //    instrument.Attack = EditorGUILayout.FloatField("Attack", instrument.Attack);
        //    instrument.Decay = EditorGUILayout.FloatField("Decay", instrument.Decay);
        //    EditorGUILayout.EndHorizontal();
        //    EditorGUILayout.BeginHorizontal();
        //    instrument.Sustain = EditorGUILayout.FloatField("Sustain", instrument.Sustain);
        //    instrument.Release = EditorGUILayout.FloatField("Release", instrument.Release);
        //    EditorGUILayout.EndHorizontal();
        //    //instrument.Volume = EditorGUILayout.Slider("Volume", instrument.Volume, Instrument.MIN_VOLUME, 6.0f);
        //}

        // Volume
        musician.MasterVolume = EditorGUILayout.Slider(new GUIContent("Volume", "Master volume in dBs."), musician.MasterVolume, AudioProperties.MIN_VOLUME_DB, AudioProperties.MAX_VOLUME_DB);

        EditorGUILayout.Space();

        // Sequencer properties
        musician.Tempo = EditorGUILayout.IntSlider(new GUIContent("Tempo", "BPM."), musician.Tempo, 88, 220);

        if (Application.isPlaying) GUI.enabled = false;
        musician.SongDuration = EditorGUILayout.Slider(new GUIContent("Song Duration", "Song duration in minutes."), musician.SongDuration, 1.0f, 10.0f);
        musician.BarsPerSection = EditorGUILayout.IntSlider(new GUIContent("Bars Per Section", "Number of bars per each section."), musician.BarsPerSection, 1, 8);
        musician.BeatsPerBar = EditorGUILayout.IntSlider(new GUIContent("Beats Per Bar", "Number of beats per each bar."), musician.BeatsPerBar, 1, 16);
        if (Application.isPlaying) GUI.enabled = true;

        EditorGUILayout.Space();

        // Key
        musician.RootNote = (NoteIndex)EditorGUILayout.EnumPopup(new GUIContent("Root Note", "Fundamental key of the song."), musician.RootNote);

        EditorGUILayout.Space();

        // Ensemble
        if (Application.isPlaying) GUI.enabled = false;
        musician.macroGeneratorType = EditorGUILayout.TextField(new GUIContent("Macro Generator", "For the musical form (sequence of sections)."), musician.macroGeneratorType);
        musician.mesoGeneratorType = EditorGUILayout.TextField(new GUIContent("Meso Generator", "For harmonic progressions (sequence of bars per section)."), musician.mesoGeneratorType);
        if (Application.isPlaying) GUI.enabled = true;

        EditorGUILayout.Space();

        // Mood selection
        musician.moodSelectionMode = (MoodSelectionMode)EditorGUILayout.EnumPopup("Mood", musician.moodSelectionMode);
        EditorGUI.indentLevel++;
        switch (musician.moodSelectionMode)
        {
            case MoodSelectionMode.Basic:
                musician.SetMood((Mood)EditorGUILayout.EnumPopup(musician.mood));
                break;

            case MoodSelectionMode.Advanced:
                float energy = EditorGUILayout.Slider("Energy", musician.Energy, 0.0f, 1.0f);
                if (energy != musician.Energy)
                {
                    musician.SetEnergy(energy);
                }
                float stress = EditorGUILayout.Slider("Stress", musician.Stress, 0.0f, 1.0f);
                if (stress != musician.Stress)
                    musician.SetStress(stress);
                break;
        }
        EditorGUI.indentLevel--;
    }

}