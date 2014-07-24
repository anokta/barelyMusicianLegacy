using UnityEngine;
using System.Collections;
using UnityEditor;
using BarelyAPI;

[CustomEditor(typeof(Ensemble))]
public class EnsembleEditor : Editor
{
    Ensemble ensemble;

    void OnEnable()
    {
        ensemble = (Ensemble)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //ensemble.initialTempo = EditorGUILayout.Slider("Tempo", ensemble.initialTempo, 100, 200);

        //ensemble.Energy = EditorGUILayout.Slider("Energy", ensemble.Energy, 0.0f, 1.0f);
        //ensemble.Stress = EditorGUILayout.Slider("Stress", ensemble.Stress, 0.0f, 1.0f);
    }
}

