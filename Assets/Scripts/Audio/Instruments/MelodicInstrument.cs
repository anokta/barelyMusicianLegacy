using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyMusician
{
    public abstract class MelodicInstrument : Instrument
    {
        [SerializeField] // TODO delete SerializeFields later
        [Range(1, 32)]
        public int voiceCount = 16;

        LinkedList<Voice> activeList, freeList;

        protected override void Awake()
        {
            base.Awake();

            freeList = new LinkedList<Voice>(voices);
            activeList = new LinkedList<Voice>();
        }

        protected override void noteOn(Note note)
        {
            Voice voice;

            if (freeList.Count > 0) // are any voices free?
            {
                voice = freeList.First.Value;

                freeList.RemoveFirst();
                activeList.AddLast(voice);
            }
            else // if not, steal the first used one
            {
                voice = activeList.First.Value;
            }

            voice.Pitch = note.Pitch;
            voice.Gain = note.Velocity;
            voice.Start();
        }

        protected override void noteOff(Note note)
        {
            foreach (Voice voice in activeList)
            {
                if (voice.Pitch == note.Pitch)
                {
                    voice.Stop();
                    activeList.Remove(voice);
                    freeList.AddLast(voice);

                    break;
                }
            }
        }

        public override void StopAllNotes()
        {
            activeList.Clear();
            freeList = new LinkedList<Voice>(voices);

            base.StopAllNotes();
        }
    }
}