using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Conductor
    {
        public enum Tempo { ANDANTE = 90, MODERATO = 120, ALLEGRO = 152 }
        public Tempo tempo;

        public float articulation;

        public float loudness;

        public float noteOnset;

        public float keyIndex;

        public ModeGenerator mode;

        public Conductor()
        {
            mode = new SimpleModeGenerator();
            mode.GenerateScale(0.0f, -1.0f);
        }

        public float GetNote(int index)
        {
            return keyIndex + mode.GetNoteOffset(index);
        }
    }
}