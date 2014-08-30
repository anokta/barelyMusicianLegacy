// ----------------------------------------------------------------------
//   Adaptive music composition engine implementation for interactive systems.
//
//     Copyright 2014 Alper Gungormusler. All rights reserved.
//
// ------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Cell
    {
        // Position
        //Vector2 position;
        //public Vector2 Position
        //{
        //    get { return position; }
        //    set { position = value; }
        //}

        // State
        int state, previousState;
        public int State
        {
            get { return previousState; }
            set { state = value; }
        }

        public Cell(int state)
        {
            State = state;

            Update();
        }

        public void Update()
        {
            previousState = state;
        }
    }
}