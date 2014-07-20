using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public abstract class MicroGenerator
    {
        protected List<NoteMeta> line;
        protected int lineLength;

        protected MicroGenerator(int length)
        {
            line = new List<NoteMeta>();
            lineLength = length;
        }

        public abstract List<NoteMeta> GenerateLine(char section, int harmonic);
    }
}