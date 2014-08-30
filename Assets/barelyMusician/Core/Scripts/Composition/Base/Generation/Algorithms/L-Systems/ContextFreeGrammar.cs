// ----------------------------------------------------------------------
//   Adaptive music composition engine implementation for interactive systems.
//
//     Copyright 2014 Alper Gungormusler. All rights reserved.
//
// ------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class ContextFreeGrammar
    {
        Dictionary<string, List<string>> rules;

        public ContextFreeGrammar()
        {
            rules = new Dictionary<string, List<string>>();
        }

        public void AddRule(string symbol, string substitutions)
        {
            rules[symbol] = new List<string>(substitutions.Split('|'));
            for (int i = 0; i < rules[symbol].Count; ++i)
                rules[symbol][i] = rules[symbol][i].Trim();
        }

        public string GenerateSequence(string startSymbol)
        {
            List<string> sequence = new List<string>();
            sequence.Add(startSymbol);

            for (int i = 0; i < sequence.Count; ++i)
            {
                if (sequence[i].Length > 1)
                {
                    string symbol = sequence[i];
                    sequence.RemoveAt(i);
                    sequence.InsertRange(i, getSubstitution(symbol));

                    i--;
                }
            }

            string result = "";
            foreach (string symbol in sequence)
                result += symbol;

            return result;
        }

        string[] getSubstitution(string symbol)
        {
            List<string> rule = null;
            if (rules.TryGetValue(symbol, out rule))
            {
                float p = RandomNumber.NextFloat();

                float cumulative = 0.0f;
                float increment = 1.0f / rule.Count;
                for (int i = 0; i < rule.Count; ++i)
                {
                    cumulative += increment;
                    if (p < cumulative)
                    {
                        return rule[i].Split(' ');
                    }
                }
            }

            return null;
        }
    }
}