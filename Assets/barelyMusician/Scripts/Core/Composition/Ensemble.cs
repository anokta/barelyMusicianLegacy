using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    //[ExecuteInEditMode]
    public class Ensemble : MonoBehaviour
    {
        // Tempo
        [SerializeField] [Range (72, 220)]
        public int initialTempo;

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

                conductor.TempoMultiplier = energy;
                sequencer.Tempo = (int)(initialTempo * conductor.TempoMultiplier);
                conductor.ArticulationMultiplier = 1.0f - energy;
                conductor.LoudnessMultiplier = energy;
                conductor.NoteOnsetMultiplier = 1.0f - energy;
                conductor.ArticulationVariance = energy;

                conductor.LoudnessVariance = (energy + stress) / 2.0f;
                conductor.HarmonicCurve = (stress > 0.5f) ? 1.0f - (0.75f * stress + 0.25f * energy) : 1.0f;
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

                conductor.PitchHeight = energy * 0.25f + (1.0f - stress) * 0.75f;
                conductor.LoudnessVariance = (energy + stress) / 2.0f;
                conductor.HarmonicCurve = (stress > 0.5f) ? 1.0f - (0.75f * stress + 0.25f * energy) : 1.0f;
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

        void OnEnable()
        {
            sequencer = new Sequencer(initialTempo, 4, 8, 8, 32);
            conductor = new Conductor((float)fundamentalKey);

            instruments = new Instrument[2];
            instruments[0] = new SamplerInstrument(sample, new Envelope(0.0f, 0.0f, 1.0f, 0.25f));
            instruments[1] = new SynthInstrument(OscillatorType.SAW, new Envelope(0.25f, 0.5f, 1.0f, 0.25f), -5.0f);
            //instruments[2] = new PercussiveInstrument(drumKit, -4.0f);
            
            producers = new Producer[instruments.Length];
            for (int i = 0; i < producers.Length; ++i)
            {
                if (i == 0)
                    producers[i] = new Producer(instruments[i], new SimpleMacroGenerator(true), new SimpleMesoGenerator(sequencer.BarCount), new SimpleMicroGenerator(sequencer.BeatCount));
                else if(i == 1)
                    producers[i] = new Producer(instruments[i], new SimpleMacroGenerator(true), new SimpleMesoGenerator(sequencer.BarCount), new CA1DMicroGenerator(sequencer.BeatCount));
                else
                    producers[i] = new Producer(instruments[i], new SimpleMacroGenerator(true), new SimpleMesoGenerator(sequencer.BarCount), new DrumsMicroGenerator(sequencer.BeatCount));
                
                producers[i].SetConductor(conductor);
                producers[i].RegisterSequencer(sequencer);
            }
        }

        void Update()
        {
            if (Mathf.Abs(energy - energyTarget) < 0.01f * energyInterpolationSpeed)
                energy = energyTarget;
            else
                energy = Mathf.Lerp(energy, energyTarget, energyInterpolationSpeed);

            if (Mathf.Abs(stress - stressTarget) < 0.01f * stressInterpolationSpeed)
                stress = stressTarget;
            else
                stress = Mathf.Lerp(stress, stressTarget, stressInterpolationSpeed);
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

        public void SetMood(Mood mood, float smoothness = 0.0f)
        {
            switch (mood)
            {
                case Mood.HAPPY:
                    SetMood(0.5f, 0.0f, smoothness);
                    break;
                case Mood.TENDER:
                    SetMood(0.0f, 0.0f, smoothness);
                    break;
                case Mood.EXCITING:
                    SetMood(1.0f, 0.0f, smoothness);
                    break;
                case Mood.SAD:
                    SetMood(0.25f, 0.75f, smoothness);
                    break;
                case Mood.DEPRESSED:
                    SetMood(0.0f, 1.0f, smoothness);
                    break;
                case Mood.ANGRY:
                    SetMood(1.0f, 1.0f, smoothness);
                    break;
            }
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
            GUILayout.Label("pitch height: " + conductor.PitchHeight);
            GUILayout.Label("harmonic complexity: " + conductor.harmonicComplexity);
            GUILayout.Label("harmonic curve: " + conductor.HarmonicCurve);
            GUILayout.Label("articulation variance: " + conductor.ArticulationVariance);
            GUILayout.Label("loudness variance: " + conductor.LoudnessVariance);
        }
    }

    public enum Mood { HAPPY, TENDER, EXCITING, SAD, DEPRESSED, ANGRY }
}