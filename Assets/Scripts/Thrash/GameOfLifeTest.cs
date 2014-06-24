using UnityEngine;
using System.Collections;
using BarelyMusician;

public class GameOfLifeTest : MonoBehaviour {

    int width, height;
    GameObject[,] cellObjects;

    Automaton2D automaton;
    GameObject cellPrefab;

	// Use this for initialization
	void Start () {
        automaton = Composer.scoreGenerator;//new Automaton2D(height, width);

        width = MainClock.BeatCount;
        height = 8;

        cellObjects = new GameObject[width, height];
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                cellObjects[x, y] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cellObjects[x, y].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                cellObjects[x, y].transform.position = cellObjects[x, y].transform.localScale.x * (automaton.GetCell(x, y).Position - new Vector2((width - 1) / 2.0f, (height - 1) / 2.0f));
            }
        }
	}

	// Update is called once per frame
	void Update () {
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                cellObjects[x, y].renderer.material.color = automaton.GetCell(x, y).State == 0 ? Color.white : Color.black;
            }
        }
	}

    //void OnNextBar(int bar)
    //{
    //    automaton.GenerateNext();
    //}
}
