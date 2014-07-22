using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Sequencer
    {
        // Event dispatcher
        public delegate void SequencerEvent(SequencerState state);
        event SequencerEvent OnNextSection, OnNextBar, OnNextBeat, OnNextPulse;
        
        // Tempo
        public int Tempo
        {
            get { return currentState.BPM; }
            set { currentState.BPM = value; }
        }

        // Number of bars per section
        public int BarCount
        {
            get { return currentState.BarCount; }
        }

        // Number of beats per bar
        public int BeatCount
        {
            get { return currentState.BeatCount; }
        }

        SequencerState currentState;
        float phasor;

        public Sequencer(int tempo = 120, int barCount = 4, int beatCount = 8, int noteType = 8, int pulseCount = 32)
        {
            currentState = new SequencerState(tempo, barCount, beatCount, noteType, pulseCount);

            Reset();
        }

        public void Reset()
        {
            currentState.Reset();

            phasor = currentState.PulseInterval;
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
                if (phasor++ >= currentState.PulseInterval)
                {
                    currentState.CurrentPulse++;

                    if (currentState.CurrentPulse % currentState.BeatLength == 0)
                    {
                        currentState.CurrentBeat++;

                        if (currentState.CurrentBeat == 0)
                        {
                            currentState.CurrentBar++;

                            if (currentState.CurrentBar == 0)
                            {
                                currentState.CurrentSection++;

                                triggerNextSection();
                            }

                            triggerNextBar();
                        }

                        triggerNextBeat();
                    }

                    triggerNextPulse();

                    phasor -= currentState.PulseInterval;
                }
            }
        }

        // Event callback functions
        void triggerNextSection()
        {
            if (OnNextSection != null)
            {
                OnNextSection(currentState);
            }
        }

        void triggerNextBar()
        {
            if (OnNextBar != null)
            {
                OnNextBar(currentState);
            }
        }

        void triggerNextBeat()
        {
            if (OnNextBeat != null)
            {
                OnNextBeat(currentState);
            }
        }

        void triggerNextPulse()
        {
            if (OnNextPulse != null)
            {
                OnNextPulse(currentState);
            }
        }
    }
}