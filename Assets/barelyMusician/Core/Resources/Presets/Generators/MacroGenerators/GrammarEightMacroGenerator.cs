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
    public class GrammarEightMacroGenerator : MacroGenerator
    {
        ContextFreeGrammar grammar;

        public GrammarEightMacroGenerator(int length, bool looping = false)
            : base(length, looping)
        {
            grammar = new ContextFreeGrammar();

            grammar.AddRule("Start", "Intro Body Outro");
            grammar.AddRule("Intro", "I");
            grammar.AddRule("Body", "Statement Repetition Cadence");
            grammar.AddRule("Statement", "V V C | V P C | V C");
            grammar.AddRule("Repetition", "V C C | V P C | V C B P C | V C B V P C | V C B V C | Repetition B Repetition");
            grammar.AddRule("Cadence", "C | P C C | C C | P C");
            grammar.AddRule("Outro", "O");
        }

        protected override void generateSequence(int length)
        {
            sectionSequence = grammar.GenerateSequence("Start");
        }
    }
}