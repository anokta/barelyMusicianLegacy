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

        protected override void generateProgression(char section, ref int[] progression)
        {
            if (section == 'A')
            {
                progression[0] = 1;
                progression[1] = 4;
                progression[2] = 2;
                progression[3] = 5;
            }
            else if (section == 'B')
            {
                progression[0] = 1;
                progression[1] = 4;
                progression[2] = 2;
                progression[3] = 1;
            }
            else
            {
                progression[0] = 1;
                progression[1] = 4;
                progression[2] = 5;
                progression[3] = 1;
            }
        }
    }
}