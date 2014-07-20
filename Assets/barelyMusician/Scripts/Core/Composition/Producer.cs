using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Producer : MonoBehaviour
    {
        // Arousal (Passive - Active)
        float energy = 0.5f;
        public float Energy
        {
            get { return energy; }
            set
            {
                energy = value;

                tempo = 72 + (int)((152 - 72) * energy);
                conductor.articulation = 1.0f - energy;
                conductor.loudness = energy;
                conductor.noteOnset = 1.0f - energy;
                articulationVariance = energy;

                loudnessVariance = (energy + stress) / 2.0f;
                harmonicCurve = (stress > 0.5f) ? (stress + energy) / 2.0f : 1.0f;
            }
        }

        // Valence (Happy - Sad) 
        float stress = 0.5f;
        public float Stress
        {
            get { return stress; }
            set
            {
                stress = value;

                harmonicComplexity = stress;
                scale = (stress < 0.25f) ? ModeGenerator.MusicalScale.MAJOR : ((stress < 0.5f) ? ModeGenerator.MusicalScale.NATURAL_MINOR : ModeGenerator.MusicalScale.HARMONIC_MINOR);
                
                pitchHeight = 1.0f - stress;
                    
                loudnessVariance = (energy + stress) / 2.0f;
                harmonicCurve = (stress > 0.5f) ? (stress + energy) / 2.0f : 1.0f;
            }
        }

        // 72 - 152
        int tempo;
        //public int Tempo
        //{
        //    get { return tempo; }
        //    set { tempo = 72 + (152 - 72) * value; }
        //}

        // 0.0f - 1.0f
        float articulation;

        // 0.0f - 1.0f
        float loudness;

        // 0.0f - 1.0f
        float noteOnset;

        // 0.0f - 1.0f
        float loudnessVariance;

        // 0.0f - 1.0f
        float articulationVariance;

        // 0.0f - 1.0f
        float harmonicComplexity;

        // 0.0f - 1.0f
        float harmonicCurve;

        // 0.0f - 1.0f
        float pitchHeight;

        ModeGenerator.MusicalScale scale;
        ModeGenerator.MusicalMode mode;

        public Composer[] composers;
        Composer composerTest;

        public Performer[] performers;
        Performer performerTest;

        Conductor conductor;

        void Start()
        {
            conductor = new Conductor();

            composerTest = composers[0];
            performerTest = performers[0];
            performerTest.conductor = conductor;

            //for (int i = 0; i < macro.SequenceLength; ++i)
            //{
            //    char currentSectionName = macro.GetSectionName(i);
            //    List<NoteInfo> currentSection = scoreSections[currentSectionName];

            //    foreach (NoteInfo noteInfo in scoreSections[currentSectionName])
            //    {
            //        performer.AddNote(new Note(keySignutare + noteInfo.index, noteInfo.velocity), i * progressionLength + noteInfo.start, noteInfo.duration);
            //    }
            //}

        }

        void OnEnable()
        {
            AudioEventManager.OnNextBar += OnNextBar;
        }

        void OnDisable()
        {
            AudioEventManager.OnNextBar -= OnNextBar;
        }

        void OnNextBar(int bar)
        {
            composerTest.GenerateNextBar(performerTest);
        }

        public void PrintValues()
        {
            GUI.color = Color.black;

            GUILayout.Label("tempo: " + tempo);
            GUILayout.Label("articulation: " + articulation);
            GUILayout.Label("loudness: " + conductor.loudness);
            GUILayout.Label("note onset: " + noteOnset);
            GUILayout.Label("pitch height: " + pitchHeight);
            GUILayout.Label("harmonic complexity: " + harmonicComplexity);
            GUILayout.Label("harmonic curve: " + harmonicCurve);
            GUILayout.Label("articulation variance: " + articulationVariance);
            GUILayout.Label("loudness variance: " + loudnessVariance);
        }
    }
}