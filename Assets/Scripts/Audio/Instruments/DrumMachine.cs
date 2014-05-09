using UnityEngine;
using System.Collections;

public class DrumMachine : MonoBehaviour
{
    public Instrument[] drumKit;
    enum DrumKit { Kick = 60, Snare = 62, HihatClosed = 71, HihatOpen = 72 }

    int[][] currentBar;


    void Start()
    {
        currentBar = new int[drumKit.Length][];
        for (int i = 0; i < drumKit.Length; ++i)
        {
            currentBar[i] = new int[16];
        }

        for (int i = 0; i < currentBar[0].Length; ++i)
        {
            currentBar[0][i] = (i % 4 == 0) ? (int)DrumKit.Kick : 0;
            currentBar[1][i] = (i % 8 == 4) ? (int)DrumKit.Snare : 0;
            currentBar[2][i] = (i % 4 == 2) ? (int)DrumKit.HihatClosed : 0;
            currentBar[3][i] = (i % 16 == 15) ? (int)DrumKit.HihatOpen : 0;
        }

        AudioEventManager.OnNextTrig += OnNextTrig;
    }

    void OnNextTrig(int clock)
    {
        for (int i = 0; i < drumKit.Length; ++i)
        {
            if (currentBar[i][clock] > 0)
            {
                drumKit[i].Trigger();
            }
        }
    }
}
    