// ----------------------------------------------------------------------
//   Adaptive music composition engine implementation for interactive systems.
//
//     Copyright 2014 Alper Gungormusler. All rights reserved.
//
// ------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Sequencer
    {
        // Event dispatcher
        public delegate void SequencerEvent(Sequencer sequencer);
        event SequencerEvent OnNextSection, OnNextBar, OnNextBeat, OnNextPulse;
        
        // Beats per minute
        int bpm;
        public int Tempo
        {
            get { return bpm; }
            set { bpm = value; }
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
        public NoteType NoteType
        {
            get { return (NoteType)noteType; }
            set { noteType = (int)value; }
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
        
        public int MinuteToSections(float minutes)
        {
            return Mathf.RoundToInt((minutes * bpm * noteType / 4.0f) / (barCount * beatCount));
        }

        float pulseInterval
        {
            get { return 240.0f * AudioProperties.SAMPLE_RATE / pulseCount / bpm; }
        }

        float phasor;

        public Sequencer(int tempo = 120, int barCount = 4, int beatCount = 4, NoteType noteType = NoteType.QUARTER_NOTE, int pulseCount = 32)
        {
            Tempo = tempo;
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

            phasor = pulseInterval;
        }

        // Event registration
        public void AddSectionListener(SequencerEvent sectionEvent)
        {
            OnNextSection += sectionEvent;
        }

        public void AddBarListener(SequencerEvent barEvent)
        {
            OnNextBar += barEvent;
        }

        public void AddBeatListener(SequencerEvent beatEvent)
        {
            OnNextBeat += beatEvent;
        }

        public void AddPulseListener(SequencerEvent pulseEvent)
        {
            OnNextPulse += pulseEvent;
        }
        
        public void RemoveSectionListener(SequencerEvent sectionEvent)
        {
            OnNextSection -= sectionEvent;
        }

        public void RemoveBarListener(SequencerEvent barEvent)
        {
            OnNextBar -= barEvent;
        }

        public void RemoveBeatListener(SequencerEvent beatEvent)
        {
            OnNextBeat -= beatEvent;
        }

        public void RemovePulseListener(SequencerEvent pulseEvent)
        {
            OnNextPulse -= pulseEvent;
        }

        // Audio callback
        public void Update(int bufferSize)
        {
            for (int i = 0; i < bufferSize; ++i)
            {
                if (phasor++ >= pulseInterval)
                {
                    CurrentPulse++;

                    if (CurrentPulse % BeatLength == 0)
                    {
                        CurrentBeat++;

                        if (CurrentBeat == 0)
                        {
                            CurrentBar++;

                            if (CurrentBar == 0)
                            {
                                CurrentSection++;

                                triggerNextSection();
                            }

                            triggerNextBar();
                        }

                        triggerNextBeat();
                    }

                    triggerNextPulse();

                    phasor -= pulseInterval;
                }
            }
        }

        // Event callback functions
        void triggerNextSection()
        {
            if (OnNextSection != null)
            {
                OnNextSection(this);
            }
        }

        void triggerNextBar()
        {
            if (OnNextBar != null)
            {
                OnNextBar(this);
            }
        }

        void triggerNextBeat()
        {
            if (OnNextBeat != null)
            {
                OnNextBeat(this);
            }
        }

        void triggerNextPulse()
        {
            if (OnNextPulse != null)
            {
                OnNextPulse(this);
            }
        }
    }

    public enum NoteType { WHOLE_NOTE = 1, HALF_NOTE = 2, QUARTER_NOTE = 4, EIGHTH_NOTE = 8, SIXTEENTH_NOTE = 16 }
}