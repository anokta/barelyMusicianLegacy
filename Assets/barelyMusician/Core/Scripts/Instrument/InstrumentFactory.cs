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

        public static Instrument CreateInstrument(InstrumentMeta meta)
        {
            Type instrumentType = Type.GetType("BarelyAPI." + InstrumentTypes[meta.Type]);
            if (instrumentType == null) instrumentType = Type.GetType("BarelyAPI.SynthInstrument");

            return (Instrument)Activator.CreateInstance(instrumentType, (System.Object)meta);
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