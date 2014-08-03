using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Beat
    {
        int bar;
        public int Bar
        {
            get { return bar; }
        }

        int index;
        public int Index
        {
            get { return index; }
        }

        int length;
        public int Length
        {
            get { return length; }
        }

        int barLength;
        public int BarLength
        {
            get { return barLength; }
        }

        public Beat(int bar, int index, int length, int barLength)
        {
            this.bar = bar;
            this.index = index;
            this.length = length;
            this.barLength = barLength;
        }
    }
}