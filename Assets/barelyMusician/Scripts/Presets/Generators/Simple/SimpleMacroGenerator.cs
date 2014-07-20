using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class SimpleMacroGenerator : MacroGenerator
    {
        public override void GenerateSequence()
        {
            sectionSequence = "ABAC";//"AAABAAAC";
        }
    }
}