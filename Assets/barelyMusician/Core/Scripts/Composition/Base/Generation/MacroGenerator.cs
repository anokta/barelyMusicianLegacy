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

        public SectionType GetSection(int index)
        {
            if (index >= sectionSequence.Length)
            {
                if (loop) index %= sectionSequence.Length;
                else return SectionType.END;
            }

            return (SectionType)sectionSequence[index];
        }

        public abstract void GenerateSequence(int length);
    }

    public enum SectionType { INTRO = 'I', VERSE = 'V', PRE_CHORUS = 'P', CHORUS = 'C', BRIDGE = 'B', OUTRO = 'O', END = '.' } 
}