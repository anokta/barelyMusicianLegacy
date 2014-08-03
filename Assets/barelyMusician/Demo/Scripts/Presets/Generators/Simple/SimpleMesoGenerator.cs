using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class SimpleMesoGenerator : MesoGenerator
    {
        public SimpleMesoGenerator(SequencerState sequencerState)
            : base(sequencerState)
        {
        }

        public override void GenerateProgression(char section)
        {
            if (section == 'A')
            {
                harmonicProgression[0] = 1;
                harmonicProgression[1] = 4;
                harmonicProgression[2] = 2;
                harmonicProgression[3] = 5;
            }
            else if (section == 'B')
            {
                harmonicProgression[0] = 1;
                harmonicProgression[1] = 4;
                harmonicProgression[2] = 2;
                harmonicProgression[3] = 1;
            }
            else
            {
                harmonicProgression[0] = 1;
                harmonicProgression[1] = 4;
                harmonicProgression[2] = 5;
                harmonicProgression[3] = 1;
            }
        }
    }
}