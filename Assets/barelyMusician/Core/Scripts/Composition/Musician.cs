using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Musician : MonoBehaviour
    {
        [SerializeField]
        [Range(88, 220)]
        public int initialTempo = 120;

        [SerializeField]
        [Range(0.0f, 10.0f)]
        public float songDuration = 1.0f;

        [SerializeField]
        [Range(1, 8)]
        public int barsPerSection = 4;

        [SerializeField]
        [Range(1, 16)]
        public int beatsPerBar = 4;

        [SerializeField]
        NoteIndex rootNote = NoteIndex.C4;
        public NoteIndex RootNote
        {
            get { return rootNote; }
            set { rootNote = value; conductor.Key = (float)rootNote; }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float masterVolume = 1.0f;
        public float MasterVolume
        {
            get { return (masterVolume != 0.0f) ? 20.0f * Mathf.Log10(masterVolume) : Instrument.MIN_VOLUME; }
            set { masterVolume = (value > Instrument.MIN_VOLUME) ? Mathf.Pow(10, 0.05f * value) : 0.0f; }
        }

        // Arousal (Passive - Active)
        float energy = 0.5f;
        float energyTarget, energyInterpolationSpeed;
        public float Energy
        {
            get { return energy; }
            set
            {
                energy = value;

                conductor.SetParameters(energy, stress);
                sequencer.Tempo = (int)(initialTempo * conductor.TempoMultiplier);
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

                conductor.SetParameters(energy, stress);
            }
        }

        #region TEST_ZONE
        public AudioClip sample, sampleBass;
        public AudioClip[] drumKit;

        #endregion TEST_ZONE

        Sequencer sequencer;
        public Sequencer Sequencer
        {
            get { return sequencer; }
        }

        Ensemble ensemble;
        Conductor conductor;

        AudioSource audioSource;

        void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.hideFlags = HideFlags.HideInInspector;
            audioSource.panLevel = 0.0f;
            audioSource.Stop();

            sequencer = new Sequencer(initialTempo, barsPerSection, beatsPerBar);
            conductor = new Conductor((float)rootNote);

            ensemble = new Ensemble(new GrammarMacroGenerator(sequencer.State.MinuteToSections(songDuration), true), new MarkovMesoGenerator(sequencer.State), conductor);
            ensemble.Register(sequencer);
        }

        void Start()
        {            
            #region TEST_ZONE
            Instrument[] instruments = new Instrument[7];
            instruments[0] = new SamplerInstrument(sample, new Envelope(0.0f, 0.0f, 1.0f, 0.25f)); instruments[0].AddEffect(new Distortion(2.0f));
            instruments[1] = new SynthInstrument(OscillatorType.COS, new Envelope(0.1f, 0.25f, 1.0f, 0.2f), -3.0f);
            instruments[2] = new SynthInstrument(OscillatorType.TRIANGLE, new Envelope(0.25f, 0.5f, 0.5f, 0.25f), -5.0f);
            instruments[3] = new PercussiveInstrument(drumKit, -3.0f);

            instruments[4] = new SamplerInstrument(sampleBass, new Envelope(0.0f, 0.0f, 1.0f, 0.25f));
            //instruments[5] = new SynthInstrument(OscillatorType.SAW, new Envelope(0.1f, 0.25f, 1.0f, 0.2f), -8.0f);
            //instruments[6] = new SynthInstrument(OscillatorType.SQUARE, new Envelope(0.25f, 0.5f, 0.5f, 0.25f), -10.0f);

            ensemble.AddPerformer("Loop", new Performer(instruments[0], new NaberMicroGenerator(sequencer.State)));
            ensemble.AddPerformer("Melody", new Performer(instruments[1], new CA1DMicroGenerator(sequencer.State)));
            ensemble.AddPerformer("Chords", new Performer(instruments[2], new ChordMicroGenerator(sequencer.State)));
            ensemble.AddPerformer("Drums", new Performer(instruments[3], new DrumsMicroGenerator(sequencer.State)));

            //ensemble.AddPerformer("Bass", new Performer(instruments[4], new NaberMicroGenerator(sequencer.State)));
            //ensemble.AddPerformer("Melody 2", new Performer(instruments[5], new NaberMicroGenerator(sequencer.State)));
            //ensemble.AddPerformer("Chords 2", new Performer(instruments[6], new ChordMicroGenerator(sequencer.State)));

            #endregion TEST_ZONE
        }

        void FixedUpdate()
        {
            if (Mathf.Abs(energy - energyTarget) < 0.01f * energyInterpolationSpeed)
                Energy = energyTarget;
            else
                Energy = Mathf.Lerp(energy, energyTarget, energyInterpolationSpeed);

            if (Mathf.Abs(stress - stressTarget) < 0.01f * stressInterpolationSpeed)
                Stress = stressTarget;
            else
                Stress = Mathf.Lerp(stress, stressTarget, stressInterpolationSpeed);
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            sequencer.Update(data.Length / channels);

            for (int i = 0; i < data.Length; i += channels)
            {
                data[i] = Mathf.Clamp(masterVolume * ensemble.GetOutput(), -1.0f, 1.0f);

                // If stereo, copy the mono data to each channel
                if (channels == 2) data[i + 1] = data[i];
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

            ensemble.Stop();
            sequencer.Reset();
        }

        public bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        public void SetMood(Mood moodType, float smoothness = 0.0f)
        {
            switch (moodType)
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
    }

    public enum Mood { HAPPY, TENDER, EXCITING, SAD, DEPRESSED, ANGRY }
}