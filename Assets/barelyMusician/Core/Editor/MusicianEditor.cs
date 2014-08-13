﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace BarelyAPI
{
    [CustomEditor(typeof(Musician))]
    public class MusicianEditor : Editor
    {
        Musician musician;

        PerformerWindow performerWindow;
        bool performerFoldout;

        int selection;

        void OnEnable()
        {
            musician = (Musician)target;
            musician.Init();

            if (performerWindow != null)
                performerWindow.musician = musician;
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            // Volume
            musician.MasterVolume = EditorGUILayout.Slider(new GUIContent("Volume", "Master volume in dBs."), musician.MasterVolume, AudioProperties.MIN_VOLUME_DB, AudioProperties.MAX_VOLUME_DB);

            EditorGUILayout.Space();

            // Sequencer properties
            GUILayout.Box("Sequencer");
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
            GUILayout.Box("Ensemble");

            if (Application.isPlaying) GUI.enabled = false;
            musician.MacroGeneratorTypeIndex = EditorGUILayout.Popup("Macro Generator", musician.MacroGeneratorTypeIndex, GeneratorFactory.MacroGeneratorTypes);
            musician.MesoGeneratorTypeIndex = EditorGUILayout.Popup("Meso Generator", musician.MesoGeneratorTypeIndex, GeneratorFactory.MesoGeneratorTypes);
            // = EditorGUILayout.TextField(new GUIContent("Macro Generator", "For the musical form (sequence of sections)."), musician.MacroGeneratorType);
            //musician.MesoGeneratorType = EditorGUILayout.TextField(new GUIContent("Meso Generator", "For harmonic progressions (sequence of bars per section)."), musician.MesoGeneratorType);
            if (Application.isPlaying) GUI.enabled = true;

            EditorGUILayout.Space();

            // Performers
            performerFoldout = EditorGUILayout.Foldout(performerFoldout, "Performers");
            if (performerFoldout)
            {
                for (int i = 0; i < musician.InstrumentName.Count; ++i)
                {
                    bool active = musician.Ensemble.IsPerformerActive(musician.InstrumentName[i]);
                    if (!active) GUI.enabled = false;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(musician.InstrumentName[i]);
                    GUILayout.FlexibleSpace();
                    if (!active) GUI.enabled = true;
                    musician.Ensemble.TogglePeformer(musician.InstrumentName[i], GUILayout.Toggle(active, ""));
                    if (GUILayout.Button("Delete"))
                    {
                        musician.DeregisterPerformer(musician.InstrumentName[i]);
                        break;
                    }
                    if (!active) GUI.enabled = false;
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal(); EditorGUILayout.PrefixLabel("Instrument Type"); EditorGUILayout.LabelField(InstrumentFactory.InstrumentTypes[musician.InstrumentType[i].type]); EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal(); EditorGUILayout.PrefixLabel("Micro Generator"); EditorGUILayout.LabelField(GeneratorFactory.MicroGeneratorTypes[musician.MicroGeneratorType[i]]); EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;

                    if (!active) GUI.enabled = true;

                    EditorGUILayout.Space();
                }

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add New Performer") && performerWindow == null)
                {
                    performerWindow = (PerformerWindow)EditorWindow.GetWindow(typeof(PerformerWindow), true, "Performer");
                    performerWindow.musician = musician;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            // Mood selection
            GUILayout.Box("Conductor");
            musician.MoodSelectionMode = (MoodSelectionMode)EditorGUILayout.EnumPopup("Mood", musician.MoodSelectionMode);
            EditorGUI.indentLevel++;
            switch (musician.MoodSelectionMode)
            {
                case MoodSelectionMode.Basic:
                    musician.SetMood((Mood)EditorGUILayout.EnumPopup(musician.Mood));
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
}