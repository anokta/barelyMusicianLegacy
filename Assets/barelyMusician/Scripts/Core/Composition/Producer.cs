using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Producer : MonoBehaviour
    {
        // Arousal
        float energy;
        public float Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        // Valence
        float stress;
        public float Stress
        {
            get { return stress; }
            set { stress = value; }
        }

        public Composer[] composers;
        Composer composerTest;

        public Performer[] performers;
        Performer performerTest;

        Conductor conductor;

        void Start()
        {
            conductor = new Conductor();

            composerTest = composers[0];
            performerTest = performers[0];

            //for (int i = 0; i < macro.SequenceLength; ++i)
            //{
            //    char currentSectionName = macro.GetSectionName(i);
            //    List<NoteInfo> currentSection = scoreSections[currentSectionName];

            //    foreach (NoteInfo noteInfo in scoreSections[currentSectionName])
            //    {
            //        performer.AddNote(new Note(keySignutare + noteInfo.index, noteInfo.velocity), i * progressionLength + noteInfo.start, noteInfo.duration);
            //    }
            //}

        }

        void OnEnable()
        {
            AudioEventManager.OnNextBar += OnNextBar;
        }

        void OnDisable()
        {
            AudioEventManager.OnNextBar -= OnNextBar;
        }

        void OnNextBar(int bar)
        {
            composerTest.PlayNextBar(performerTest, conductor.mode, conductor.keySignutare);
        }
    }
}