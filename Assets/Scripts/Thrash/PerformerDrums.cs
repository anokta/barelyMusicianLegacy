using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class PerformerDrums : MonoBehaviour
    {
        Instrument instrument;

        Note[][] currentBar;
        bool change;

        void Start()
        {
            instrument = GameObject.Find("Drum Machine").GetComponent<PercussiveInstrument>();

            currentBar = new Note[4][];
            for (int i = 0; i < 4; ++i)
            {
                currentBar[i] = new Note[32];
            }

            for (int i = 0; i < currentBar[0].Length; ++i)
            {
                currentBar[0][i] = new Note((i % 4 == 0) ? 36 : 0, 1.0f);
                currentBar[1][i] = new Note((i % 8 == 4) ? 37 : 0, 1.0f);
                currentBar[2][i] = new Note((i % 4 == 2) ? 38 : 0, RandomNumber.NextFloat(0.5f, 1.0f));
                currentBar[3][i] = new Note((i % 16 == 15) ? 39 : 0, 1.0f);
            }
        }

        void Update()
        {
            if (change)
            {
                change = false;
                for (int i = 0; i < currentBar[0].Length; ++i)
                {
                    currentBar[0][i] = new Note((RandomNumber.NextFloat(0.0f, 1.0f) < 0.025f || (RandomNumber.NextFloat(0.0f, 1.0f) < 0.96f && (i % 4 == 0))) ? 36 : 0, 1.0f);
                    currentBar[1][i] = new Note((RandomNumber.NextFloat(0.0f, 1.0f) < 0.025f || (i % 8 == 4)) ? 37 : 0, 1.0f);
                    currentBar[2][i] = new Note((RandomNumber.NextFloat(0.0f, 1.0f) < 0.01f || (i % 4 == 2)) ? 38 : 0, RandomNumber.NextFloat(0.5f, 1.0f));
                    currentBar[3][i] = new Note((RandomNumber.NextFloat(0.0f, 1.0f) < 0.01f || (i % 16 == 15)) ? 39 : 0, 1.0f);
                }
            }
        }

        void OnEnable()
        {
            AudioEventManager.OnNextPulse += OnNextPulse;
            AudioEventManager.OnNextBar += OnNextBar;
        }

        void OnDisable()
        {
            AudioEventManager.OnNextPulse -= OnNextPulse;
            AudioEventManager.OnNextBar -= OnNextBar;
        }

        void OnNextBar(int bar)
        {
            change = true;
        }

        void OnNextPulse(int pulse)
        {
            pulse--;
            if (pulse % 2 == 0)
            {
                for (int i = 0; i < 4; ++i)
                {
                    if (currentBar[i][pulse / 2].Index > 0)
                        instrument.PlayNote(currentBar[i][pulse / 2]);
                }
            }
        }
    }
}