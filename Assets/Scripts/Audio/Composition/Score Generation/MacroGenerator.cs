using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class MacroGenerator
    {
        protected string sectionSequence;
        public int SequenceLength
        {
            get { return sectionSequence.Length; }
        }

        public MacroGenerator()
        {
        }

        public void GenerateSequence()
        {
            sectionSequence = "AAAB";
        }

        public char GetSectionName(int index)
        {
            return sectionSequence[index];
        }
    }
}