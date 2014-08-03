using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using BarelyAPI;

[CustomEditor(typeof(Producer))]
public class ProducerEditor : Editor
{
    Producer producer;
    List<Instrument> instruments;

    void OnEnable()
    {
        producer = (Producer)target;

        instruments = new List<Instrument>();
        //instruments.Add(new SynthInstrument(OscillatorType.SAW, new Envelope(0,0,0,0)));
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        foreach (Instrument instrument in instruments)
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

        //producer.initialTempo = EditorGUILayout.Slider("Tempo", producer.initialTempo, 100, 200);

        //producer.Energy = EditorGUILayout.Slider("Energy", producer.Energy, 0.0f, 1.0f);
        //producer.Stress = EditorGUILayout.Slider("Stress", producer.Stress, 0.0f, 1.0f);
    }
}