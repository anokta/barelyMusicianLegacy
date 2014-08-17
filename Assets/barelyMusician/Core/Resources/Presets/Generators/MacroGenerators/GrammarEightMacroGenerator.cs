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
            grammar.AddRule("Repetition", "V P C C | V C C | V V C C | V P C | Repetition Repetition | Repetition B Repetition");
            grammar.AddRule("Cadence", "C | C C | P C C");
            grammar.AddRule("Outro", "O");
        }

        protected override void generateSequence(int length)
        {
            sectionSequence = grammar.GenerateSequence("Start");
        }
    }
}