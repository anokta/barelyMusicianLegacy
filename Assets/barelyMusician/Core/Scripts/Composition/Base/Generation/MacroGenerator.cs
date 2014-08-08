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

        int targetLength;
        bool loop;

        public MacroGenerator(int length, bool looping = false)
        {
            targetLength = length;
            loop = looping;

            Restart();
        }

        public SectionType GetSection(int index)
        {
            if (sectionSequence.Length == 0)
                generateSequence(targetLength);

            if (index >= sectionSequence.Length)
            {
                if (loop) index %= sectionSequence.Length;
                else return SectionType.END;
            }

            return (SectionType)sectionSequence[index];
        }

        public void Restart()
        {
            sectionSequence = "";
        }

        protected abstract void generateSequence(int length);
    }

    public enum SectionType { INTRO = 'I', VERSE = 'V', PRE_CHORUS = 'P', CHORUS = 'C', BRIDGE = 'B', OUTRO = 'O', END = '.' } 
}