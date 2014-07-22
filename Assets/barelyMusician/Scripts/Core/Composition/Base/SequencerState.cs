using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class SequencerState
    {
        // Beats per minute
        int bpm;
        public int BPM
        {
            get { return bpm; }
            set { bpm = value; }
        }

        // Current state
        int currentSection;
        public int CurrentSection
        {
            get { return currentSection; }
            set { currentSection = value; }
        }

        int currentBar;
        public int CurrentBar
        {
            get { return currentBar; }
            set { currentBar = value % barCount; }
        }

        int currentBeat;
        public int CurrentBeat
        {
            get { return currentBeat; }
            set { currentBeat = value % beatCount; }
        }

        int currentPulse;
        public int CurrentPulse
        {
            get { return currentPulse; }
            set { currentPulse = value % BarLength; }
        }

        // Bars per section
        int barCount;
        public int BarCount
        {
            get { return barCount; }
            set { barCount = value; } 
        }

        // Beats per bar
        int beatCount;
        public int BeatCount
        {
            get { return beatCount; }
            set { beatCount = value; }
        }

        // Clock frequency per bar
        int pulseCount;
        public int PulseCount
        {
            get { return pulseCount; }
            set { pulseCount = value; }
        }

        // Note type (quarter, eigth etc.)
        int noteType;
        public int NoteType
        {
            get { return noteType; }
            set { noteType = value; }
        }

        // Section length (in pulses)
        public int SectionLength
        {
            get { return barCount * BarLength; }
        }

        // Bar length (in pulses)
        public int BarLength
        {
            get { return beatCount * BeatLength; }
        }

        // Beat length (in pulses)
        public int BeatLength
        {
            get { return pulseCount / noteType; }
        }

        public float PulseInterval
        {
            get { return 240.0f * AudioProperties.SampleRate / pulseCount / bpm; }
        }

        public SequencerState(int tempo, int barCount, int beatCount, int noteType, int pulseCount)
        {
            BPM = tempo;
            BarCount = barCount;
            BeatCount = beatCount;
            NoteType = noteType;
            PulseCount = pulseCount;

            Reset();
        }

        public void Reset()
        {
            currentSection = -1;
            currentBar = -1;
            currentBeat = -1;
            currentPulse = -1;
        }
    }
}