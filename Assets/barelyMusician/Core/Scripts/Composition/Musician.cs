using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Musician : MonoBehaviour
    {
        public GeneratorFactory GeneratorFactory;
        public InstrumentFactory InstrumentFactory;

        public int MacroGeneratorTypeIndex = 0;
        public int MesoGeneratorTypeIndex = 0;

        public MoodSelectionMode MoodSelectionMode;
        public Mood Mood;

        public List<string> InstrumentName;
        public List<int> InstrumentType;
        public List<int> MicroGeneratorType;

        // Tempo (BPM)
        [SerializeField]
        int initialTempo = 120;
        public int Tempo
        {
            get { return initialTempo; }
            set
            {
                initialTempo = value;
                sequencer.Tempo = (int)(initialTempo * conductor.TempoMultiplier);
            }
        }

        // Song duration in minutes
        [SerializeField]
        float songDuration = 1.0f;
        public float SongDuration
        {
            get { return songDuration; }
            set { songDuration = value; ensemble.SongDurationInSections = sequencer.MinuteToSections(songDuration); }
        }

        // Bars per section
        [SerializeField]
        int barsPerSection = 4;
        public int BarsPerSection
        {
            get { return barsPerSection; }
            set { barsPerSection = value; sequencer.BarCount = barsPerSection; }
        }

        // Beats per bar
        [SerializeField]
        public int beatsPerBar = 4;
        public int BeatsPerBar
        {
            get { return beatsPerBar; }
            set { beatsPerBar = value; sequencer.BeatCount = beatsPerBar; }
        }

        // Fundamental key of the song
        [SerializeField]
        NoteIndex rootNote = NoteIndex.C4;
        public NoteIndex RootNote
        {
            get { return rootNote; }
            set { rootNote = value; conductor.Key = (float)rootNote; }
        }

        // Master volume
        [SerializeField]
        float masterVolume = 1.0f;
        public float MasterVolume
        {
            get { return (masterVolume != 0.0f) ? 20.0f * Mathf.Log10(masterVolume) : float.NegativeInfinity; }
            set { masterVolume = (value > AudioProperties.MIN_VOLUME_DB) ? Mathf.Pow(10, 0.05f * value) : 0.0f; }
        }

        // Arousal (Passive - Active)
        [SerializeField]
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
        [SerializeField]
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
        public Ensemble Ensemble
        {
            get { return ensemble; }
        }

        Conductor conductor;

        AudioSource audioSource;
        public bool IsPlaying
        {
            get { return audioSource.isPlaying; }
        }

        void Awake()
        {
            Init();

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.hideFlags = HideFlags.HideInInspector;
            audioSource.panLevel = 0.0f;
            audioSource.Stop();
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

            //ensemble.AddPerformer("Loop", new Performer(instruments[0], new NaberMicroGenerator(sequencer)));
            //ensemble.AddPerformer("Melody", new Performer(instruments[1], new CA1DMicroGenerator(sequencer)));
            //ensemble.AddPerformer("Chords", new Performer(instruments[2], new ChordMicroGenerator(sequencer)));
            //ensemble.AddPerformer("Drums", new Performer(instruments[3], new DrumsMicroGenerator(sequencer)));

            //ensemble.AddPerformer("Bass", new Performer(instruments[4], new NaberMicroGenerator(sequencer)));
            //ensemble.AddPerformer("Melody 2", new Performer(instruments[5], new NaberMicroGenerator(sequencer)));
            //ensemble.AddPerformer("Chords 2", new Performer(instruments[6], new ChordMicroGenerator(sequencer)));

            #endregion TEST_ZONE
        }

        public void RegisterPerformer(string instrumentName, int instrumentType, int microGeneratorTypeIndex)
        {
            InstrumentName.Add(instrumentName);
            InstrumentType.Add(instrumentType);
            MicroGeneratorType.Add(microGeneratorTypeIndex);
        }

        public void DeregisterPerformer(string instrumentName)
        {
            int index = InstrumentName.IndexOf(instrumentName);
            if (index > -1)
            {
                InstrumentName.RemoveAt(index);
                InstrumentType.RemoveAt(index);
                MicroGeneratorType.RemoveAt(index);
            }
        }

        void FixedUpdate()
        {
            if (energy != energyTarget)
            {
                if (Mathf.Abs(energy - energyTarget) < 0.01f * energyInterpolationSpeed)
                    Energy = energyTarget;
                else
                    Energy = Mathf.Lerp(energy, energyTarget, energyInterpolationSpeed);
            }
            if (stress != stressTarget)
            {
                if (Mathf.Abs(stress - stressTarget) < 0.01f * stressInterpolationSpeed)
                    Stress = stressTarget;
                else
                    Stress = Mathf.Lerp(stress, stressTarget, stressInterpolationSpeed);
            }
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

        public void Init()
        {
            if (sequencer == null) sequencer = new Sequencer(initialTempo, barsPerSection, beatsPerBar);
            if (conductor == null) conductor = new Conductor((float)rootNote);

            if (GeneratorFactory == null) GeneratorFactory = ScriptableObject.CreateInstance<GeneratorFactory>();
            if (InstrumentFactory == null) InstrumentFactory = ScriptableObject.CreateInstance<InstrumentFactory>();

            if (ensemble == null)
            {
                MacroGenerator macro = GeneratorFactory.CreateMacroGenerator(MacroGeneratorTypeIndex, sequencer.MinuteToSections(songDuration), true);
                MesoGenerator meso = GeneratorFactory.CreateMesoGenerator(MesoGeneratorTypeIndex, sequencer);

                ensemble = new Ensemble(macro, meso, conductor);
                ensemble.Register(sequencer);
                
                // performers
                if (InstrumentName == null) InstrumentName = new List<string>();
                if (InstrumentType == null) InstrumentType = new List<int>();
                if (MicroGeneratorType == null) MicroGeneratorType = new List<int>();

                for (int i = 0; i < InstrumentName.Count; ++i)
                {
                    string name = InstrumentName[i];

                    Instrument instrument = InstrumentFactory.CreateInstrument(InstrumentType[i]);
                    MicroGenerator micro = GeneratorFactory.CreateMicroGenerator(MicroGeneratorType[i], sequencer);

                    ensemble.AddPerformer(name, new Performer(instrument, micro));
                }
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

        public void SetMood(Mood moodType, float smoothness = 0.0f)
        {
            Mood = moodType;
            switch (Mood)
            {
                case Mood.Happy:
                    SetMood(0.5f, 0.0f, smoothness);
                    break;
                case Mood.Tender:
                    SetMood(0.0f, 0.0f, smoothness);
                    break;
                case Mood.Exciting:
                    SetMood(1.0f, 0.0f, smoothness);
                    break;
                case Mood.Sad:
                    SetMood(0.25f, 0.75f, smoothness);
                    break;
                case Mood.Depressed:
                    SetMood(0.0f, 1.0f, smoothness);
                    break;
                case Mood.Angry:
                    SetMood(1.0f, 1.0f, smoothness);
                    break;
                default:
                    SetMood(0.5f, 0.5f, smoothness);
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

            if (smoothness == 0.0f & Energy != energy) Energy = energyTarget;
        }

        public void SetStress(float stress, float smoothness = 0.0f)
        {
            stressTarget = stress;
            stressInterpolationSpeed = (smoothness == 0.0f) ? 1.0f : Time.deltaTime / (smoothness * smoothness);

            if (smoothness == 0.0f & Stress != stress) Stress = stressTarget;
        }
    }

    public enum Mood { Happy, Tender, Exciting, Sad, Depressed, Angry }
    public enum MoodSelectionMode { Basic, Advanced }
}