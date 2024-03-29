﻿// ----------------------------------------------------------------------
//   Adaptive music composition engine implementation for interactive systems.
//
//     Copyright 2014 Alper Gungormusler. All rights reserved.
//
// ------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BarelyAPI
{
    public class UBasslineMicroGenerator : MicroGenerator
    {
        public UBasslineMicroGenerator(Sequencer sequencer)
            : base(sequencer)
        {
        }

        protected override void generateLine(SectionType section, int bar, int harmonic, ref List<NoteMeta> line)
        {
            if (section == SectionType.INTRO)
            {
                if (bar % 2 == 0) line.Add(new NoteMeta(0, 0.0f, 1.0f, 0.86f));
            }
            else
            {
                line.Add(new NoteMeta(harmonic, 0.0f, 0.5f, RandomNumber.NextFloat(0.85f, 0.90f)));
                line.Add(new NoteMeta(harmonic, 0.5f, 0.5f, RandomNumber.NextFloat(0.85f, 0.90f)));

                if (section == SectionType.CHORUS)
                {
                    line.Add(new NoteMeta(harmonic + (2 * bar) % ModeGenerator.SCALE_LENGTH, 0.75f, 0.25f, RandomNumber.NextFloat(0.85f, 0.90f)));
                    //line.Add(new NoteMeta(harmonic, 0.5f, 0.5f, RandomNumber.NextFloat(0.85f, 0.90f)));
                }
            }
        }
    }
}