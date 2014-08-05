using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class SimpleMacroGenerator : MacroGenerator
    {
        public SimpleMacroGenerator(bool looping = false)
            : base(looping)
        {
        }

        public override void GenerateSequence(int length)
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

            Debug.Log(sectionSequence);
        }
    }
}