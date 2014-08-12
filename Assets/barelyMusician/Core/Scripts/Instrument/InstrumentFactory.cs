using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace BarelyAPI
{
    [Serializable]
    public class InstrumentFactory : ScriptableObject
    {
        public string[] InstrumentTypes;

        void OnEnable()
        {
            findInstrumentTypes();
        }

        void findInstrumentTypes()
        {
            string[] folderMacro = { "Assets/barelyMusician/Demo/Scripts/Presets/Instruments" };

            InstrumentTypes = AssetDatabase.FindAssets("", folderMacro);
            for (int i = 0; i < InstrumentTypes.Length; ++i)
            {
                string fullPath = AssetDatabase.GUIDToAssetPath(InstrumentTypes[i]);
                InstrumentTypes[i] = fullPath.Substring(fullPath.LastIndexOf('/') + 1, fullPath.LastIndexOf('.') - fullPath.LastIndexOf('/') - 1);
            }
        }

        public Instrument CreateInstrument(int typeIndex)
        {
            return createInstrument(InstrumentTypes[typeIndex]);
        }

        Instrument createInstrument(string type)
        {
            Type instrumentType = Type.GetType("BarelyAPI." + type); if (instrumentType == null) instrumentType = Type.GetType("BarelyAPI.SynthInstrument");
            return (Instrument)System.Activator.CreateInstance(instrumentType, OscillatorType.SQUARE, new Envelope(0.1f, 0.25f, 1.0f, 0.2f), -3.0f, 16);
        }
    }
}