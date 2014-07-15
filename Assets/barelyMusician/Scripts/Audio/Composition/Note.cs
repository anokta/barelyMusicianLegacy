using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Note
    {
        // Pitch (Hz)
        float frequency;
        public float Pitch
        {
            get { return frequency; }
        }

        // Index (default = A4)
        float index;
        public float Index
        {
            get { return index; }
            set { index = value; frequency = Mathf.Pow(2, index / 12) * 440.0f; }
        }

        // Loudness (0. - 1.)
        float loudness;
        public float Loudness
        {
            get { return loudness; }
            set { loudness = value; }
        }
        //public int Velocity
        //{
        //    get { return (int)(loudness * 127); }
        //    set { loudness = value / 127.0f; }
        //}

        public bool IsNoteOn
        {
            get { return loudness > 0.0f; }
        }

        public Note(float index, float loudness = 1.0f)
        {
            Index = index;

            Loudness = loudness;
        }
    }

    public enum NoteIndex
    {
        A4 = 0, B4 = 2, C4 = 3, D4 = 5, E4 = 7, F4 = 8, G4 = 10
    }

}