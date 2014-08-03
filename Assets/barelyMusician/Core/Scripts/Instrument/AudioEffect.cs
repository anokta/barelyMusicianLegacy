using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public abstract class AudioEffect
    {
        protected bool enabled;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        protected AudioEffect()
        {
            enabled = true;
        }

        public abstract void Apply(TimbreProperties timbreProperties);

        public abstract float Process(float sample);
    }
}