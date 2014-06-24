using UnityEngine;
using System.Collections;

namespace BarelyMusician
{
    public class Cell
    {
        // Position
        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        // State
        int state, previousState;
        public int State
        {
            get { return previousState; }
            set { state = value; }
        }

        public Cell(Vector2 position, int state)
        {
            Position = position;
            State = state;

            Update();
        }

        public void Update()
        {
            previousState = state;
        }
    }
}