using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class MusicianEditorProperties : ScriptableObject
    {
        public string MacroGeneratorType;
        public string MesoGeneratorType;

        public MoodSelectionMode MoodSelectionMode;
        public Mood Mood;

        public List<string> InstrumentNames;
        public List<string> InstrumentTypes;
        public List<string> MicroGeneratorTypes;

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }

        //public Ensemble InstantiateEnsemble()
        //{
        //        Type macroType = Type.GetType("BarelyAPI." + MacroGeneratorType); if (macroType == null) macroType = Type.GetType("BarelyAPI.SimpleMacroGenerator");
        //        MacroGenerator macro = (MacroGenerator)System.Activator.CreateInstance(macroType, sequencer.MinuteToSections(songDuration), true);

        //        Type mesoType = Type.GetType("BarelyAPI." + MesoGeneratorType); if (mesoType == null) mesoType = Type.GetType("BarelyAPI.SimpleMesoGenerator");
        //        MesoGenerator meso = (MesoGenerator)System.Activator.CreateInstance(mesoType, sequencer);

        //        Ensemble ensemble = new Ensemble(macro, meso, conductor);
        //        ensemble.Register(sequencer);

        //        // performers
        //        if (instrumentNames == null) instrumentNames = new List<string>();
        //        if (instrumentTypes == null) instrumentTypes = new List<string>();
        //        if (microGeneratorTypes == null) microGeneratorTypes = new List<string>();

        //        for (int i = 0; i < instrumentNames.Count; ++i)
        //        {
        //            string name = instrumentNames[i];

        //            Type instrumentType = Type.GetType("BarelyAPI." + instrumentTypes[i]); if (instrumentType == null) instrumentType = Type.GetType("BarelyAPI.SynthInstrument");
        //            Instrument instrument = (Instrument)System.Activator.CreateInstance(instrumentType, OscillatorType.COS, new Envelope(0.1f, 0.25f, 1.0f, 0.2f), -3.0f, 16);

        //            Type microType = Type.GetType("BarelyAPI." + microGeneratorTypes[i]); if (microType == null) microType = Type.GetType("BarelyAPI.SimpleMicroGenerator");
        //            MicroGenerator micro = (MicroGenerator)System.Activator.CreateInstance(microType, sequencer);

        //            ensemble.AddPerformer(name, new Performer(instrument, micro));
        //        }
        //}
    }
}