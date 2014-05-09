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
            currentBar[i] = new int[8];
        }

        for (int i = 0; i < currentBar[0].Length; ++i)
        {
            currentBar[0][i] = (i % 2 == 0) ? (int)DrumKit.Kick : 0;
            currentBar[1][i] = (i % 4 == 2) ? (int)DrumKit.Snare : 0;
            currentBar[2][i] = (i % 2 == 1) ? (int)DrumKit.HihatClosed : 0;
            currentBar[3][i] = (i % 8 == 7) ? (int)DrumKit.HihatOpen : 0;
        }

        AudioEventManager.OnNextBeat += OnNextBeat;
    }

    void OnNextBeat()
    {
        for (int i = 0; i < drumKit.Length; ++i)
        {
            if (currentBar[i][MainClock.beatCount-1] > 0)
            {
                drumKit[i].Trigger();
            }
        }
    }
}
    