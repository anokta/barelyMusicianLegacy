using UnityEngine;
using System.Collections;
using BarelyAPI;

public class BunnyGUI : MonoBehaviour
{

    public GUIText startGUI, scoreGUI, bunnyScoreGUI;
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

        bunnyScoreGUI.text = "BUNNYSCORE: " + PlayerPrefs.GetInt("bunnyscore", 0);
        bunnyScoreGUI.enabled = true;
    }

    void GameStart()
    {
        startGUI.enabled = false;
        scoreGUI.enabled = true;
        bunnyScoreGUI.enabled = false;

        score = 0;
    }

    void GameOver()
    {
        if (PlayerPrefs.GetInt("bunnyscore", 0) < score)
        {
            PlayerPrefs.SetInt("bunnyscore", score);
            PlayerPrefs.Save();
        }

        bunnyScoreGUI.text = "BUNNYSCORE: " + PlayerPrefs.GetInt("bunnyscore", 0);

        bunnyScoreGUI.enabled = true;
        startGUI.text = "SPACE to Restart";
        startGUI.enabled = true;
    }

    void OnNextBeat(Sequencer sequencer)
    {
        if(GameEventManager.CurrentState == GameEventManager.GameState.Running)
            score += 10;
    }
}