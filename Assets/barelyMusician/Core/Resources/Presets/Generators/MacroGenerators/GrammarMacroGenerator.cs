// ----------------------------------------------------------------------
//   Adaptive music composition engine implementation for interactive systems.
//
//     Copyright 2014 Alper Gungormusler. All rights reserved.
//
// ------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class GrammarMacroGenerator : MacroGenerator
    {
        ContextFreeGrammar grammar;

        public GrammarMacroGenerator(int length, bool looping = false)
            : base(length, looping)
        {
            grammar = new ContextFreeGrammar();

            grammar.AddRule("Start", "Intro Body Outro");
            grammar.AddRule("Intro", "I");
            grammar.AddRule("Body", "Statement Repetition Cadence");
            grammar.AddRule("Statement", "V V V P C C | V V V V C C | V V C");
            grammar.AddRule("Repetition", "V P C C | V V C C | V P C | Repetition Repetition | Repetition B Repetition");
            grammar.AddRule("Cadence", "C | C C | P C C");
            grammar.AddRule("Outro", "O");
        }

        protected override void generateSequence(int length)
        {
            sectionSequence = grammar.GenerateSequence("Start");
        }
    }
}