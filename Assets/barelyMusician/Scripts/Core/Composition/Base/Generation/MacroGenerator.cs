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

        public char GetSectionName(int index)
        {
            return sectionSequence[index];
        }

        public abstract void GenerateSequence();
    }
}