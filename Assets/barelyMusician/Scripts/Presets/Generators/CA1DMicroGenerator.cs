using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class CA1DMicroGenerator : MicroGenerator
    {
        Automaton1D ca;

        public CA1DMicroGenerator(int length)
            : base(length)
        {
            ca = new Automaton1D(81, 90);
        }

        public override List<NoteMeta> GenerateLine(char section, int harmonic)
        {
            line = new List<NoteMeta>();

            int keyIndex = harmonic;

            ca.Update();

            for (int i = 0; i < lineLength; ++i)
            {
                if (ca.GetState(i) == 1)
                {
                    line.Add(new NoteMeta(keyIndex++, (float)i / lineLength, 1.0f / lineLength, 1.0f));
                }
            }
            
            return line;
        }
    }
}