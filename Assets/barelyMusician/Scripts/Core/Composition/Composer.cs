using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Composer
    {
        public MacroGenerator macro;
        public MesoGenerator meso;
        public MicroGenerator micro;

        public int progressionLength;

        Dictionary<char, List<NoteMeta>[]> scoreSections;

        public Composer()//MacroGenerator macroGenerator, MesoGenerator mesoGenerator, MicroGenerator microGenerator)
        {
            scoreSections = new Dictionary<char, List<NoteMeta>[]>();
            progressionLength = 4;

            macro = new SimpleMacroGenerator();
            meso = new SimpleMesoGenerator(progressionLength);
            micro = new SimpleMicroGenerator(MainClock.BeatCount);

            macro.GenerateSequence();
        }

        public void GenerateNextSection(int currentBar)
        {
            int section = (currentBar-1) / progressionLength;

            if (section >= macro.SequenceLength) return;

            List<NoteMeta>[] currentSection;
            char currentSectionName = macro.GetSectionName(section);

            if (!scoreSections.TryGetValue(currentSectionName, out currentSection))
            {
                currentSection = new List<NoteMeta>[progressionLength];

                meso.GenerateProgression(currentSectionName);

                for (int i = 0; i < meso.ProgressionLength; ++i)
                {
                    currentSection[i] = micro.GenerateLine(currentSectionName, meso.GetHarmonic(i));
                }

                scoreSections[currentSectionName] = currentSection;
            }
        }

        public void AddNextBeat(Performer performer, int currentBar, int beat)
        {
            int section = (currentBar - 1) / progressionLength;

            if (section >= macro.SequenceLength) return;

            List<NoteMeta>[] currentSection;
            char currentSectionName = macro.GetSectionName(section);

            if (scoreSections.TryGetValue(currentSectionName, out currentSection))
            {
                foreach (NoteMeta meta in currentSection[(currentBar - 1) % progressionLength])
                {
                    if (meta.Offset >= (float)(beat) / MainClock.BeatCount && meta.Offset < (float)(beat + 1) / MainClock.BeatCount)
                        performer.AddNote(meta);
                }
            }
        }
    }
}