using UnityEngine;
using System;
using System.Collections;

namespace BarelyAPI
{
    [Serializable]
    public class GeneratorFactory : ScriptableObject
    {
        public string[] MacroGeneratorTypes;
        public string[] MesoGeneratorTypes;
        public string[] MicroGeneratorTypes;

        void OnEnable()
        {
            getGeneratorTypes("MacroGenerators");
            getGeneratorTypes("MesoGenerators");
            getGeneratorTypes("MicroGenerators");
        }

        public MacroGenerator CreateMacroGenerator(int typeIndex, int sequenceLength, bool loop = true)
        {
            return createMacroGenerator(MacroGeneratorTypes[typeIndex], sequenceLength, loop);
        }

        public MesoGenerator CreateMesoGenerator(int typeIndex, Sequencer sequencer)
        {
            return createMesoGenerator(MesoGeneratorTypes[typeIndex], sequencer);
        }

        public MicroGenerator CreateMicroGenerator(int typeIndex, Sequencer sequencer)
        {
            return createMicroGenerator(MicroGeneratorTypes[typeIndex], sequencer);
        }

        MacroGenerator createMacroGenerator(string type, int sequenceLength, bool loop)
        {
            Type macroType = Type.GetType("BarelyAPI." + type); if (macroType == null) macroType = Type.GetType("BarelyAPI.SimpleMacroGenerator");
            return (MacroGenerator)System.Activator.CreateInstance(macroType, sequenceLength, loop);
        }

        MesoGenerator createMesoGenerator(string type, Sequencer sequencer)
        {
            Type mesoType = Type.GetType("BarelyAPI." + type); if (mesoType == null) mesoType = Type.GetType("BarelyAPI.SimpleMesoGenerator");
            return (MesoGenerator)System.Activator.CreateInstance(mesoType, sequencer);
        }

        MicroGenerator createMicroGenerator(string type, Sequencer sequencer)
        {
            Type microType = Type.GetType("BarelyAPI." + type); if (microType == null) microType = Type.GetType("BarelyAPI.SimpleMicroGenerator");
            return (MicroGenerator)System.Activator.CreateInstance(microType, sequencer);
        }

        void getGeneratorTypes(string type)
        {
            UnityEngine.Object[] assets = Resources.LoadAll("Presets/Generators/" + type);

            string[] types = new string[assets.Length];
            for (int i = 0; i < types.Length; ++i)
            {
                types[i] = assets[i].name;
            }

            switch (type)
            {
                case "MacroGenerator":
                    MacroGeneratorTypes = types;
                    break;
                case "MesoGenerator":
                    MesoGeneratorTypes = types;
                    break;
                case "MicroGenerator":
                    MicroGeneratorTypes = types;
                    break;
            }

            Resources.UnloadUnusedAssets();
        }
    }
}