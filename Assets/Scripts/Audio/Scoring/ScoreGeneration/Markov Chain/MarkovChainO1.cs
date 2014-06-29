using UnityEngine;
using System.Collections;
using System;

namespace BarelyAPI
{
    public class MarkovChainO1
    {
        float[,] states;

        int currentStateIndex;
        public int CurrentState
        {
            get { return currentStateIndex; }
        }

        static float[,] majorStates =
        {
            // C        D   E       F       G       A   B       C
            { 0.025f, 0.08f, 0.01f, 0.35f, 0.32f, 0.07f, 0.02f, 0.125f },   // C
            { 0.06f, 0.02f, 0.015f, 0.35f, 0.32f, 0.07f, 0.02f, 0.1f },     // D
            { 0.05f, 0.08f, 0.01f, 0.35f, 0.32f, 0.07f, 0.02f, 0.1f },      // E    
            { 0.36f, 0.08f, 0.11f, 0.01f, 0.25f, 0.07f, 0.02f, 0.1f },      // F
            { 0.17f, 0.07f, 0.01f, 0.12f, 0.04f, 0.43f, 0.01f, 0.15f },     // G
            { 0.32f, 0.08f, 0.01f, 0.05f, 0.35f, 0.03f, 0.01f, 0.15f },     // A
            { 0.05f, 0.08f, 0.01f, 0.35f, 0.32f, 0.07f, 0.02f, 0.1f },      // B
            { 0.05f, 0.08f, 0.01f, 0.35f, 0.32f, 0.07f, 0.02f, 0.1f }       // C
        };

        // TODO : Implement different orders
        public MarkovChainO1(int scaleLength, int order = 1)
        {
            states = new float[(int)Mathf.Pow(scaleLength, order), scaleLength];

            for (int i = 0; i < states.GetLength(0); ++i)
            {
                for (int j = 0; j < states.GetLength(1); ++j)
                {
                    states[i, j] = majorStates[i, j];
                }
            }

            currentStateIndex = 0;
        }

        public void GenerateNextState()
        {
            double p = RandomNumber.NextFloat();

            float cumulative = 0.0f;
            for (int i = 0; i < states.GetLength(1); ++i)
            {
                cumulative += states[currentStateIndex, i];
                if (p < cumulative)
                {
                    currentStateIndex = i;
                    break;
                }
            }
        }
    }
}