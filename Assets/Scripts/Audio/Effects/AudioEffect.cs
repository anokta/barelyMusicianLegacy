using UnityEngine;
using System.Collections;

public interface AudioEffect
{
    void Process(ref float[] data);
}
