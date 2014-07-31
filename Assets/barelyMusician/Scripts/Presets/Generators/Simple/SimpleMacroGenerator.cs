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

        public override void GenerateSequence()
        {
            sectionSequence = "AAABAAAC";
        }
    }
}