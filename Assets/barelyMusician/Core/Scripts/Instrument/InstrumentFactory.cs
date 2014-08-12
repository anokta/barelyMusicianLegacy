using UnityEngine;
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
            getInstrumentTypes();
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

        void getInstrumentTypes()
        {
            UnityEngine.Object[] assets = Resources.LoadAll("Presets/Instruments");

            InstrumentTypes = new string[assets.Length];
            for (int i = 0; i < InstrumentTypes.Length; ++i)
            {
                InstrumentTypes[i] = assets[i].name;
            }

            Resources.UnloadUnusedAssets();
        }
    }
}