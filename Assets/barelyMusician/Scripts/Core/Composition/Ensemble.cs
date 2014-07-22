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

                sequencer.Tempo = 90 + (int)((135 - 90) * energy);
                conductor.ArticulationMultiplier = 1.0f - energy;
                conductor.LoudnessMultiplier = energy;
                conductor.NoteOnsetMultiplier = 1.0f - energy;
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

        public AudioClip sample;
        public AudioClip[] drumKit;

        public Instrument[] instruments;
        Producer[] producers;

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

            instruments = new Instrument[3];
            instruments[0] = new SamplerInstrument(sample, new Envelope(0.0f, 0.0f, 1.0f, 0.25f));
            instruments[1] = new SynthInstrument(Oscillator.OSCType.SAW, new Envelope(0.25f, 0.5f, 1.0f, 0.25f), -5.0f);
            instruments[2] = new PercussiveInstrument(drumKit, -4.0f);
            
            producers = new Producer[instruments.Length];
            for (int i = 0; i < producers.Length; ++i)
            {
                if (i == 0)
                    producers[i] = new Producer(instruments[i], new SimpleMacroGenerator(), new SimpleMesoGenerator(sequencer.BarCount), new SimpleMicroGenerator(sequencer.BeatCount));
                else if(i == 1)
                    producers[i] = new Producer(instruments[i], new SimpleMacroGenerator(), new SimpleMesoGenerator(sequencer.BarCount), new CA1DMicroGenerator(sequencer.BeatCount));
                else
                    producers[i] = new Producer(instruments[i], new SimpleMacroGenerator(), new SimpleMesoGenerator(sequencer.BarCount), new DrumsMicroGenerator(sequencer.BeatCount));
                
                producers[i].SetConductor(conductor);
                producers[i].RegisterSequencer(sequencer);
            }
        }

        public void Play()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        public void Pause()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        public void Stop()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            sequencer.Reset();
            foreach (Producer producer in producers)
                producer.Reset();
        }

        public bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            sequencer.Update(data.Length / channels);

            for (int i = 0; i < data.Length; i += channels)
            {
                float output = 0.0f;

                foreach (Producer producer in producers)
                {
                    output += producer.GetOutput();
                }
                data[i] = output;

                // If stereo, copy the mono data to each channel
                if (channels == 2) data[i + 1] = data[i];
            }
        }

        public void PrintValues()
        {
            GUI.color = Color.black;

            GUILayout.Label("tempo: " + sequencer.Tempo);
            GUILayout.Label("articulation: " + conductor.ArticulationMultiplier);
            GUILayout.Label("loudness: " + conductor.LoudnessMultiplier);
            GUILayout.Label("note onset: " + conductor.NoteOnsetMultiplier);
            GUILayout.Label("pitch height: " + pitchHeight);
            GUILayout.Label("harmonic complexity: " + harmonicComplexity);
            GUILayout.Label("harmonic curve: " + harmonicCurve);
            GUILayout.Label("articulation variance: " + articulationVariance);
            GUILayout.Label("loudness variance: " + loudnessVariance);
        }



    }
}