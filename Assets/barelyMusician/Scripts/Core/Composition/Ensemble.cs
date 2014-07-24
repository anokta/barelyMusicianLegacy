using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Ensemble : MonoBehaviour
    {
        // Tempo
        [SerializeField] [Range (72, 220)]
        public int initialTempo;
        public float TempoMultiplier
        {
            set { sequencer.Tempo = Mathf.FloorToInt(initialTempo * (0.9f + 0.2f * value)); }
        }

        // Key note
        public NoteIndex fundamentalKey;

        // Arousal (Passive - Active)
        float energy = 0.5f;
        float energyTarget, energyInterpolationSpeed;
        public float Energy
        {
            get { return energy; }
            set
            {
                energy = value;

                TempoMultiplier = energy;
                conductor.ArticulationMultiplier = 1.0f - energy;
                conductor.LoudnessMultiplier = energy;
                conductor.NoteOnsetMultiplier = 1.0f - energy;
                conductor.articulationVariance = energy;

                conductor.loudnessVariance = (energy + stress) / 2.0f;
                conductor.harmonicCurve = (stress > 0.5f) ? (stress + energy) / 2.0f : 1.0f;
            }
        }

        // Valence (Happy - Sad) 
        float stress = 0.5f;
        float stressTarget, stressInterpolationSpeed;
        public float Stress
        {
            get { return stress; }
            set
            {
                stress = value;

                conductor.harmonicComplexity = stress;
                conductor.Mode = stress;

                conductor.pitchHeight = 1.0f - stress;

                conductor.loudnessVariance = (energy + stress) / 2.0f;
                conductor.harmonicCurve = (stress > 0.5f) ? (stress + energy) / 2.0f : 1.0f;
            }
        }

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
            sequencer = new Sequencer(initialTempo, 4, 8, 8, 32);
            conductor = new Conductor();
            conductor.fundamentalKey = (int)fundamentalKey;

            instruments = new Instrument[2];
            instruments[0] = new SamplerInstrument(sample, new Envelope(0.0f, 0.0f, 1.0f, 0.25f));
            instruments[1] = new SynthInstrument(OscillatorType.SAW, new Envelope(0.25f, 0.5f, 1.0f, 0.25f), -5.0f);
            //instruments[2] = new PercussiveInstrument(drumKit, -4.0f);
            
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

        void Update()
        {
            if (energy != energyTarget)
                energy = Mathf.Lerp(energy, energyTarget, energyInterpolationSpeed);
            if (stress != stressTarget)
                stress = Mathf.Lerp(stress, stressTarget, stressInterpolationSpeed);

            //if (RandomNumber.NextFloat() < 0.01f)
            //    SetMood(RandomNumber.NextFloat(), RandomNumber.NextFloat(), RandomNumber.NextFloat());
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

        public void SetMood(float energy, float stress, float smoothness = 0.0f)
        {
            SetEnergy(energy, smoothness);
            SetStress(stress, smoothness);
        }

        public void SetEnergy(float energy, float smoothness = 0.0f)
        {
            energyTarget = energy;
            energyInterpolationSpeed = (smoothness == 0.0f) ? 1.0f : Time.deltaTime / (smoothness * smoothness);
        }

        public void SetStress(float stress, float smoothness = 0.0f)
        {
            stressTarget = stress;
            stressInterpolationSpeed = (smoothness == 0.0f) ? 1.0f : Time.deltaTime / (smoothness * smoothness);
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
            GUILayout.Label("pitch height: " + conductor.pitchHeight);
            GUILayout.Label("harmonic complexity: " + conductor.harmonicComplexity);
            GUILayout.Label("harmonic curve: " + conductor.harmonicCurve);
            GUILayout.Label("articulation variance: " + conductor.articulationVariance);
            GUILayout.Label("loudness variance: " + conductor.loudnessVariance);
        }



    }
}