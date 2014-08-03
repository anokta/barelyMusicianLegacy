using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public abstract class MacroGenerator
    {
        protected string sectionSequence;
        public int SequenceLength
        {
            get { return sectionSequence.Length; }
        }

        bool loop;

        public MacroGenerator(bool looping = false)
        {
            loop = looping;
        }

        public char GetSectionName(int index)
        {
            if (index >= sectionSequence.Length)
            {
                if (loop) index %= sectionSequence.Length;
                else return '.';
            }

            return sectionSequence[index];
        }

        public abstract void GenerateSequence(int length);
    }
}