using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class Section
    {
        List<NoteMeta>[] score;

        public Section(int sectionLength)
        {
            score = new List<NoteMeta>[sectionLength];
        }

        public void AddBar(int index, List<NoteMeta> bar)
        {
            score[index] = bar;
        }

        public List<NoteMeta> GetBar(int index)
        {
            return score[index];
        }
    }
}