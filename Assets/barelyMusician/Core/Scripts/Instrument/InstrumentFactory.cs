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
            Type instrumentType = Type.GetType("BarelyAPI." + InstrumentTypes[meta.type]);
            if (instrumentType == null) instrumentType = Type.GetType("BarelyAPI.SynthInstrument");

            switch (InstrumentTypes[meta.type])
            {
                case "PercussiveInstrument":
                    return (Instrument)System.Activator.CreateInstance(instrumentType, meta.samples, meta.volume, meta.sustained, 0);

                case "SamplerInstrument":
                    return (Instrument)System.Activator.CreateInstance(instrumentType, meta.sample, new Envelope(meta.attack, meta.decay, meta.sustain, meta.release), meta.volume, 0, meta.sustained, meta.voiceCount);

                case "SynthInstrument":
                default:
                    return (Instrument)System.Activator.CreateInstance(instrumentType, meta.oscType, new Envelope(meta.attack, meta.decay, meta.sustain, meta.release), meta.volume, meta.voiceCount);
            }
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