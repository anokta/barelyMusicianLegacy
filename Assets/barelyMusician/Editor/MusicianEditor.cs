using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using BarelyAPI;

[CustomEditor(typeof(Musician))]
public class MusicianEditor : Editor
{
    Musician musician;
    List<Instrument> instruments;

    void OnEnable()
    {
        musician = (Musician)target;

        instruments = new List<Instrument>();
        //instruments.Add(new SynthInstrument(OscillatorType.SAW, new Envelope(0,0,0,0)));
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        foreach(Instrument instrument in instruments)
        {
            EditorGUILayout.BeginHorizontal();
            instrument.Attack = EditorGUILayout.FloatField("Attack", instrument.Attack);
            instrument.Decay = EditorGUILayout.FloatField("Decay", instrument.Decay);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            instrument.Sustain = EditorGUILayout.FloatField("Sustain", instrument.Sustain);
            instrument.Release = EditorGUILayout.FloatField("Release", instrument.Release);
            EditorGUILayout.EndHorizontal();
            instrument.Volume = EditorGUILayout.Slider("Volume", instrument.Volume, Instrument.MIN_VOLUME, 6.0f);
        }
            
        //musician.initialTempo = EditorGUILayout.Slider("Tempo", musician.initialTempo, 100, 200);

        //musician.Energy = EditorGUILayout.Slider("Energy", musician.Energy, 0.0f, 1.0f);
        //musician.Stress = EditorGUILayout.Slider("Stress", musician.Stress, 0.0f, 1.0f);
    }
}

