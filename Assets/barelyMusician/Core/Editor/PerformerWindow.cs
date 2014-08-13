using UnityEngine;
using System.Collections;
using UnityEditor;

namespace BarelyAPI
{
    public class PerformerWindow : EditorWindow
    {
        public Musician musician;

        public string performerName;
        public int microGeneratorType;
        public InstrumentMeta instrumentMeta;
        
        bool advanced;
        bool sampleListFoldout = true;

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
            instrumentMeta.Volume = EditorGUILayout.Slider("Volume", instrumentMeta.Volume, AudioProperties.MIN_VOLUME_DB, AudioProperties.MAX_VOLUME_DB);
            EditorGUILayout.Space();
            instrumentMeta.Type = EditorGUILayout.Popup("Type", instrumentMeta.Type, InstrumentFactory.InstrumentTypes);
            EditorGUILayout.Space();
            switch (InstrumentFactory.InstrumentTypes[instrumentMeta.Type])
            {
                case "PercussiveInstrument":
                    if (instrumentMeta.Samples == null) instrumentMeta.Samples = new AudioClip[4];

                    sampleListFoldout = EditorGUILayout.Foldout(sampleListFoldout, "Samples");
                    if (sampleListFoldout)
                    {
                        EditorGUI.indentLevel++;
                        for (int i = 0; i < instrumentMeta.Samples.Length; ++i)
                            instrumentMeta.Samples[i] = (AudioClip)EditorGUILayout.ObjectField(((DRUM_KIT)i).ToString(), instrumentMeta.Samples[i], typeof(AudioClip), false);
                        EditorGUI.indentLevel--;
                    }

                        instrumentMeta.Sustained = EditorGUILayout.Toggle("Sustained", instrumentMeta.Sustained);

                    break;

                case "SamplerInstrument":
                    instrumentMeta.Sample = (AudioClip)EditorGUILayout.ObjectField("Sample", instrumentMeta.Sample, typeof(AudioClip), false);
                    instrumentMeta.Sustained = EditorGUILayout.Toggle("Loop", instrumentMeta.Sustained);
                    EditorGUILayout.Space();

                    advanced = EditorGUILayout.Foldout(advanced, "Advanced");
                    if (advanced)
                    {
                        instrumentMeta.VoiceCount = EditorGUILayout.IntSlider("Voice Count", instrumentMeta.VoiceCount, 1, 32);

                        EditorGUILayout.BeginHorizontal();
                        instrumentMeta.Attack = EditorGUILayout.FloatField("Attack", instrumentMeta.Attack);
                        instrumentMeta.Decay = EditorGUILayout.FloatField("Decay", instrumentMeta.Decay);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        instrumentMeta.Sustain = EditorGUILayout.FloatField("Sustain", instrumentMeta.Sustain);
                        instrumentMeta.Release = EditorGUILayout.FloatField("Release", instrumentMeta.Release);
                        EditorGUILayout.EndHorizontal();
                    }
                    break;

                case "SynthInstrument":
                    instrumentMeta.OscType = (OscillatorType)EditorGUILayout.EnumPopup("Oscillator Type", instrumentMeta.OscType);

                    EditorGUILayout.Space();

                    advanced = EditorGUILayout.Foldout(advanced, "Advanced");
                    if (advanced)
                    {
                        instrumentMeta.VoiceCount = EditorGUILayout.IntSlider("Voice Count", instrumentMeta.VoiceCount, 1, 32);

                        EditorGUILayout.BeginHorizontal();
                        instrumentMeta.Attack = EditorGUILayout.FloatField("Attack", instrumentMeta.Attack);
                        instrumentMeta.Decay = EditorGUILayout.FloatField("Decay", instrumentMeta.Decay);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        instrumentMeta.Sustain = EditorGUILayout.FloatField("Sustain", instrumentMeta.Sustain);
                        instrumentMeta.Release = EditorGUILayout.FloatField("Release", instrumentMeta.Release);
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