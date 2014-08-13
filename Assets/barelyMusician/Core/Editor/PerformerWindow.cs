using UnityEngine;
using System.Collections;
using UnityEditor;

namespace BarelyAPI
{
    [System.Serializable]
    public class PerformerWindow : EditorWindow
    {

        public Musician musician;

        string instrumentName;
        int instrumentType, microGeneratorType;
        float attack, decay, sustain, release;

        void OnEnable()
        {
            //ShowAsDropDown(position, new Vector2(position.width, position.height));
        }

        void OnGUI()
        {
            instrumentName = EditorGUILayout.TextField(new GUIContent("Instrument Name", "String identifier of the instrument."), instrumentName);
            instrumentType = EditorGUILayout.Popup("Instrument Type", instrumentType, InstrumentFactory.InstrumentTypes);
            microGeneratorType = EditorGUILayout.Popup("Micro Generator", microGeneratorType, GeneratorFactory.MicroGeneratorTypes);

            EditorGUILayout.BeginHorizontal();
            attack = EditorGUILayout.FloatField("Attack", attack);
            decay = EditorGUILayout.FloatField("Decay", decay);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            sustain = EditorGUILayout.FloatField("Sustain", sustain);
            release = EditorGUILayout.FloatField("Release", release);
            EditorGUILayout.EndHorizontal();
            //instrument.Volume = EditorGUILayout.Slider("Volume", instrument.Volume, Instrument.MIN_VOLUME, 6.0f);

            EditorGUILayout.Space();

            if (GUILayout.Button("Cancel"))
            {
                Close();
            }
            if (GUILayout.Button("OK"))
            {
                musician.RegisterPerformer(instrumentName, instrumentType, microGeneratorType);

                Close();
            }
        }
    }
}