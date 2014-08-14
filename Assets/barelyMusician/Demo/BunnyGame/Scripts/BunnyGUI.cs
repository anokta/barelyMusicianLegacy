using UnityEngine;
using System.Collections;
using BarelyAPI;

public class BunnyGUI : MonoBehaviour
{

    public GUIText startGUI, scoreGUI;
    int score;

    Musician musician;

    // Use this for initialization
    void Start()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;

        musician = FindObjectOfType<Musician>();
        musician.Sequencer.AddBeatListener(OnNextBeat);
    }

    // Update is called once per frame
    void Update()
    {
        scoreGUI.text = "SCORE: " + score;
    }

    void GameMenu()
    {
        startGUI.text = "SPACE to Play";
        startGUI.enabled = true;
        scoreGUI.enabled = false;
    }

    void GameStart()
    {
        startGUI.enabled = false;
        scoreGUI.enabled = true;

        score = 0;
    }

    void GameOver()
    {
        startGUI.text = "SPACE to Restart";
        startGUI.enabled = true;
    }

    void OnNextBeat(Sequencer sequencer)
    {
        if(GameEventManager.CurrentState == GameEventManager.GameState.Running)
            score += 10;
    }
}