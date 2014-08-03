using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Producer
    {
        MicroGenerator lineGenerator;

        Performer performer;

        public Producer(Instrument instrument, MicroGenerator microGenerator)
        {
            performer = new Performer(instrument);

            lineGenerator = microGenerator;

            Reset();
        }

        public void Reset()
        {
            performer.Restart();
        }

        public void Mute(bool enabled)
        {
            performer.Mute = enabled;
        }

        public float GetOutput()
        {
            return performer.Output;
        }

        /**
         * Generate next bar if needed.
         **/
        public List<NoteMeta> GenerateBar(char section, int index, int harmonic)
        {
            return lineGenerator.GenerateLine(section, index, harmonic);
        }

        /**
         * Register next beat to performer
         **/
        public void AddBeat(List<NoteMeta> line, Beat beat, Conductor conductor)
        {
            foreach (NoteMeta noteMeta in line)
            {
                if (Mathf.FloorToInt(noteMeta.Offset * beat.Length) - beat.Index == 0)
                {
                    performNote(conductor.TransformNote(noteMeta), beat);
                }
            }
         }

        /**
         * Play next pulse
         **/
        public void PlayPulse(int bar, int pulse, TimbreProperties timbre)
        {
            performer.ApplyTransformation(timbre);
            performer.Play(bar, pulse);
        }

        void performNote(NoteMeta meta, Beat beat)
        {
            float start = beat.Bar + meta.Offset;

            performer.AddNote(new Note(meta.Index, meta.Loudness), start, meta.Duration, beat.BarLength);
        }
    }
}