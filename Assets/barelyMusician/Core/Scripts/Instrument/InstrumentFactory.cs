using UnityEngine;
using System;
using System.Collections;

namespace BarelyAPI
{
    public class InstrumentFactory
    {
        private string[] instrumentTypes;
        public static string[] InstrumentTypes
        {
            get { return instance.instrumentTypes; }
        }

        private static InstrumentFactory _instance;
        private static InstrumentFactory instance
        {
            get
            {
                if (_instance == null) 
                    _instance = new InstrumentFactory();

                return _instance;
            }
        }

        InstrumentFactory()
        {
            setInstrumentTypes();

            Resources.UnloadUnusedAssets();
        }

        public static Instrument CreateInstrument(int typeIndex)
        {
            return createInstrument(InstrumentTypes[typeIndex]);
        }

        static Instrument createInstrument(string type)
        {
            Type instrumentType = Type.GetType("BarelyAPI." + type); if (instrumentType == null) instrumentType = Type.GetType("BarelyAPI.SynthInstrument");
            return (Instrument)System.Activator.CreateInstance(instrumentType, OscillatorType.SQUARE, new Envelope(0.1f, 0.25f, 1.0f, 0.2f), -3.0f, 16);
        }

        void setInstrumentTypes()
        {
            UnityEngine.Object[] assets = Resources.LoadAll("Presets/Instruments");

            instrumentTypes = new string[assets.Length];
            for (int i = 0; i < instrumentTypes.Length; ++i)
            {
                instrumentTypes[i] = assets[i].name;
            }
        }
    }
}