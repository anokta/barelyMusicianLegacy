using UnityEngine;
using System.Collections;

namespace BarelyAPI
{
    public class Automaton2D
    {
        int width, height;
        Cell[,] cells;

        public Automaton2D(int w, int h)
        {
            width = w;
            height = h;

            cells = new Cell[width, height];

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    cells[x, y] = new Cell(Random.Range(0, 2));
                }
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

        public Cell GetCell(int x, int y)
        {
            return cells[x, y];
        }

        void applyTransitions()
        {
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    //// OPTIONAL ////
                    // Bypass boundaries
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        continue;
                    ////////

                    // Traverse neighbors
                    int neighbourCount = 0;
                    for (int ny = -1; ny <= 1; ++ny)
                    {
                        for (int nx = -1; nx <= 1; ++nx)
                        {
                            if (nx != 0 || ny != 0)
                            {
                                if (cells[(x+nx+width) % width, (y+ny+height) % height].State == 1) neighbourCount++;
                            }
                        }
                    }

                    // Transition states
                    switch (cells[x, y].State)
                    {
                        case 0: // dead
                            if (neighbourCount == 3)
                            {
                                cells[x, y].State = 1;
                            }

                            break;

                        case 1: // alive
                            if (neighbourCount < 2 || neighbourCount > 3)
                            {
                                cells[x, y].State = 0;
                            }

                            break;
                    }
                }
            }
        }
    }
}