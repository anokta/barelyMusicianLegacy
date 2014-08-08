using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class SimpleMacroGenerator : MacroGenerator
    {
        public SimpleMacroGenerator(int length, bool looping = false)
            : base(length, looping)
        {
        }

        protected override void generateSequence(int length)
        {
            sectionSequence += (char)SectionType.INTRO;
            sectionSequence += (char)SectionType.VERSE;
            sectionSequence += (char)SectionType.VERSE;
            sectionSequence += (char)SectionType.VERSE;
            sectionSequence += (char)SectionType.PRE_CHORUS;
            sectionSequence += (char)SectionType.CHORUS;
            sectionSequence += (char)SectionType.CHORUS;
            sectionSequence += (char)SectionType.VERSE;
            sectionSequence += (char)SectionType.PRE_CHORUS;
            sectionSequence += (char)SectionType.CHORUS;
            sectionSequence += (char)SectionType.BRIDGE;
            sectionSequence += (char)SectionType.PRE_CHORUS;
            sectionSequence += (char)SectionType.CHORUS;
            sectionSequence += (char)SectionType.CHORUS;
            sectionSequence += (char)SectionType.OUTRO;
        }
    }
}