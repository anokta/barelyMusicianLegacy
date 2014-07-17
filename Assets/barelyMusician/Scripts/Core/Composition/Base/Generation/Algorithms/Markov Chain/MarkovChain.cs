using UnityEngine;
using System.Collections;
using System;

namespace BarelyAPI
{
    public class MarkovChain
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
            { 0.02f, 0.08f, 0.03f, 0.36f, 0.35f, 0.10f, 0.02f, 0.04f },   // C
            { 0.18f, 0.02f, 0.015f, 0.35f, 0.27f, 0.07f, 0.02f, 0.03f },     // D
            { 0.1f, 0.08f, 0.01f, 0.35f, 0.32f, 0.07f, 0.02f, 0.05f },      // E    
            { 0.31f, 0.08f, 0.11f, 0.01f, 0.28f, 0.11f, 0.05f, 0.05f },      // F
            { 0.17f, 0.07f, 0.01f, 0.23f, 0.03f, 0.38f, 0.01f, 0.10f },     // G
            { 0.13f, 0.12f, 0.01f, 0.29f, 0.32f, 0.01f, 0.04f, 0.08f },     // A
            { 0.05f, 0.08f, 0.01f, 0.15f, 0.42f, 0.17f, 0.02f, 0.10f },      // B
            { 0.15f, 0.08f, 0.01f, 0.15f, 0.32f, 0.22f, 0.05f, 0.02f }       // C
        };

        // TODO : Implement different orders
        public MarkovChain(int scaleLength, int order = 1)
        {
            states = new float[(int)Mathf.Pow(scaleLength, order), scaleLength];

            for (int i = 0; i < states.GetLength(0); ++i)
            {
                for (int j = 0; j < states.GetLength(1); ++j)
                {
                    states[i, j] = majorStates[i, j];
                }
            }

            Reset();
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

        public void Reset()
        {
            currentStateIndex = 0;
        }
    }
}