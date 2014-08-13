using UnityEngine;
using System.Collections;
using UnityEditor;

namespace BarelyAPI
{
    public class PerformerWindow : EditorWindow
    {
        public Musician musician;

        string performerName;
        InstrumentMeta instrumentMeta;
        int microGeneratorType;

        bool advanced;
        bool sampleListFoldout;

        void OnEnable()
        {
            instrumentMeta = ScriptableObject.CreateInstance<InstrumentMeta>();
            //ShowAsDropDown(position, new Vector2(position.width, position.height));
        }

        void OnGUI()
        {
            performerName = EditorGUILayout.TextField(new GUIContent("Name", "String identifier of the performer."), performerName);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Instrument");
            EditorGUI.indentLevel++;
            instrumentMeta.volume = EditorGUILayout.Slider("Volume", instrumentMeta.volume, AudioProperties.MIN_VOLUME_DB, AudioProperties.MAX_VOLUME_DB);
            EditorGUILayout.Space();
            instrumentMeta.type = EditorGUILayout.Popup("Type", instrumentMeta.type, InstrumentFactory.InstrumentTypes);
            switch (InstrumentFactory.InstrumentTypes[instrumentMeta.type])
            {
                case "PercussiveInstrument":
                    if (instrumentMeta.samples == null) instrumentMeta.samples = new AudioClip[4];

                    sampleListFoldout = EditorGUILayout.Foldout(sampleListFoldout, "Samples");
                    if (sampleListFoldout)
                    {
                        EditorGUI.indentLevel++;
                        for (int i = 0; i < instrumentMeta.samples.Length; ++i)
                            instrumentMeta.samples[i] = (AudioClip)EditorGUILayout.ObjectField(i.ToString(), instrumentMeta.samples[i], typeof(AudioClip));
                        EditorGUI.indentLevel--;
                    }

                        instrumentMeta.sustained = EditorGUILayout.Toggle("Sustained", instrumentMeta.sustained);

                    break;

                case "SamplerInstrument":
                    instrumentMeta.sample = (AudioClip)EditorGUILayout.ObjectField("Sample", instrumentMeta.sample, typeof(AudioClip));
                    instrumentMeta.sustained = EditorGUILayout.Toggle("Loop", instrumentMeta.sustained);
                    EditorGUILayout.Space();

                    advanced = EditorGUILayout.Foldout(advanced, "Advanced");
                    if (advanced)
                    {
                        instrumentMeta.voiceCount = EditorGUILayout.IntSlider("Voice Count", instrumentMeta.voiceCount, 1, 32);

                        EditorGUILayout.BeginHorizontal();
                        instrumentMeta.attack = EditorGUILayout.FloatField("Attack", instrumentMeta.attack);
                        instrumentMeta.decay = EditorGUILayout.FloatField("Decay", instrumentMeta.decay);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        instrumentMeta.sustain = EditorGUILayout.FloatField("Sustain", instrumentMeta.sustain);
                        instrumentMeta.release = EditorGUILayout.FloatField("Release", instrumentMeta.release);
                        EditorGUILayout.EndHorizontal();
                    }
                    break;

                case "SynthInstrument":
                    instrumentMeta.oscType = (OscillatorType)EditorGUILayout.EnumPopup("Oscillator Type", instrumentMeta.oscType);

                    EditorGUILayout.Space();

                    advanced = EditorGUILayout.Foldout(advanced, "Advanced");
                    if (advanced)
                    {
                        instrumentMeta.voiceCount = EditorGUILayout.IntSlider("Voice Count", instrumentMeta.voiceCount, 1, 32);

                        EditorGUILayout.BeginHorizontal();
                        instrumentMeta.attack = EditorGUILayout.FloatField("Attack", instrumentMeta.attack);
                        instrumentMeta.decay = EditorGUILayout.FloatField("Decay", instrumentMeta.decay);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        instrumentMeta.sustain = EditorGUILayout.FloatField("Sustain", instrumentMeta.sustain);
                        instrumentMeta.release = EditorGUILayout.FloatField("Release", instrumentMeta.release);
                        EditorGUILayout.EndHorizontal();
                    }
                    break;
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            microGeneratorType = EditorGUILayout.Popup("Micro Generator", microGeneratorType, GeneratorFactory.MicroGeneratorTypes);

            EditorGUILayout.Space();

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
            {
                Close();
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("OK"))
            {
                musician.RegisterPerformer(performerName, instrumentMeta, microGeneratorType);

                Close();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
    }
}