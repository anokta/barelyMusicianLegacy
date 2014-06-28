using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Automaton1D
    {
        Cell[] cells;

        int[] ruleset;
        int r;

        public int Size
        {
            get { return cells.Length; }
        }

        public int R
        {
            get { return r; }
            set { r = value; }
        }


        public Automaton1D(int length, int rule, int r = 1)
        {
            cells = new Cell[length];
            for (int i = 0; i < cells.Length; ++i)
            {
                cells[i] = new Cell(Random.Range(0, 2));
            }

            R = r;

            ruleset = new int[(int)Mathf.Pow(2, 2 * r + 1)];

            string ruleString = System.Convert.ToString(rule, 2).PadLeft(ruleset.Length, '0');
            for (int i = 0; i < ruleset.Length; ++i)
            {
                ruleset[i] = int.Parse(ruleString[ruleset.Length - 1 - i].ToString());
            }
            
        }

        public void Update()
        {
            foreach (Cell cell in cells)
            {
                cell.Update();
            }

            applyTransitions();
        }

        public int GetState(int index)
        {
            return cells[index].State;
        }

        void applyTransitions()
        {
            for (int i = 0; i < Size; ++i)
            {
                // Traverse neighbors
                string neighbourCount = "";
                for (int j = -r; j <= r; ++j)
                {
                    neighbourCount += cells[(i + j + Size) % Size].State;
                }

                // Apply transition
                cells[i].State = ruleset[System.Convert.ToInt32(neighbourCount, 2)];
            }
        }
    }
}