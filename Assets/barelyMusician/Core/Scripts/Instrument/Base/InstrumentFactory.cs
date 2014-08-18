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

        private string[] effectTypes;
        public static string[] EffectTypes
        {
            get { return instance.effectTypes; }
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
            setTypes("Instruments");
            setTypes("Effects");

            Resources.UnloadUnusedAssets();
        }

        public static Instrument CreateInstrument(InstrumentMeta meta)
        {
            Type instrumentType = Type.GetType("BarelyAPI." + InstrumentTypes[meta.Type]);
            if (instrumentType == null) instrumentType = Type.GetType("BarelyAPI.SynthInstrument");

            return (Instrument)Activator.CreateInstance(instrumentType, (System.Object)meta);
        }

        public static AudioEffect CreateEffect(int type)
        {
            Type effectType = Type.GetType("BarelyAPI." + EffectTypes[type]);
            if (effectType == null) effectType = Type.GetType("BarelyAPI.Distortion");

            return (AudioEffect)Activator.CreateInstance(effectType);
        }

        void setTypes(string type)
        {
            UnityEngine.Object[] assets = Resources.LoadAll("Presets/" + type);

            string[] types = new string[assets.Length];
            for (int i = 0; i < types.Length; ++i)
            {
                types[i] = assets[i].name;
            }

            switch (type)
            {
                case "Instruments":
                    instrumentTypes = types;
                    break;
                case "Effects":
                    effectTypes = types;
                    break;
            }
        }

    }
}