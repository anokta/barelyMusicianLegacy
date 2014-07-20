using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class NoteMeta
    {
        public int Index;
        public float Offset;
        public float Duration;
        public float Loudness;

        public NoteMeta(int index, float offset, float duration, float loudness = 1.0f)
        {
            Index = index;
            Offset = offset;
            Duration = duration;
            Loudness = loudness;
        }
    }
}