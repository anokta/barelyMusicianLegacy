using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Ensemble : MonoBehaviour
    {
        // Arousal (Passive - Active)
        float energy = 0.5f;
        public float Energy
        {
            get { return energy; }
            set
            {
                energy = value;

                sequencer.Tempo = 72 + (int)((152 - 72) * energy);
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
                conductor.mode.setScale((stress < 0.25f) ? ModeGenerator.MusicalScale.MAJOR : ((stress < 0.5f) ? ModeGenerator.MusicalScale.NATURAL_MINOR : ModeGenerator.MusicalScale.HARMONIC_MINOR));

                pitchHeight = 1.0f - stress;

                loudnessVariance = (energy + stress) / 2.0f;
                harmonicCurve = (stress > 0.5f) ? (stress + energy) / 2.0f : 1.0f;
            }
        }

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

        Sequencer sequencer;

        Conductor conductor;

        public Instrument[] instruments;
        Producer producerTest;


        AudioSource audioSource;

        void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.hideFlags = HideFlags.HideInInspector;
            audioSource.panLevel = 0.0f;
            audioSource.Stop();
        }

        void Start()
        {
            sequencer = new Sequencer(120, 4, 8, 8, 32);
            conductor = new Conductor();

            producerTest = new Producer(instruments[0], new SimpleMacroGenerator(), new SimpleMesoGenerator(sequencer.BarCount), new SimpleMicroGenerator(sequencer.BeatCount));
            producerTest.conductor = conductor;
            producerTest.RegisterSequencer(sequencer);
        }

        public void Play()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();

                //performerTest.Start();
            }
        }

        public void Pause()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();

                //performerTest.Pause();
            }
        }

        public void Stop()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();

                //performerTest.Stop();
            }

            sequencer.Reset();
        }

        public bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            sequencer.Update(data.Length / channels);
        }

        public void PrintValues()
        {
            GUI.color = Color.black;

            GUILayout.Label("tempo: " + sequencer.Tempo);
            GUILayout.Label("articulation: " + conductor.articulation);
            GUILayout.Label("loudness: " + conductor.loudness);
            GUILayout.Label("note onset: " + conductor.noteOnset);
            GUILayout.Label("pitch height: " + pitchHeight);
            GUILayout.Label("harmonic complexity: " + harmonicComplexity);
            GUILayout.Label("harmonic curve: " + harmonicCurve);
            GUILayout.Label("articulation variance: " + articulationVariance);
            GUILayout.Label("loudness variance: " + loudnessVariance);
        }



    }
}