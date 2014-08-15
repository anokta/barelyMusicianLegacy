using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GameManager : MonoBehaviour
{

    Musician musician;

    void Awake()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
    }

    void Start()
    {
        musician = FindObjectOfType<Musician>();
        GameEventManager.TriggerGameMenu();
    }

    void Update()
    {
        switch (GameEventManager.CurrentState)
        {
            case GameEventManager.GameState.InMenu:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameEventManager.TriggerGameStart();
                }
                break;

            case GameEventManager.GameState.Running:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    GameEventManager.TriggerGameOver();
                    GameEventManager.TriggerGameMenu();
                }
                break;

            case GameEventManager.GameState.Over:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameEventManager.TriggerGameStart();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    GameEventManager.TriggerGameMenu();
                }
                break;
        }
    }

    void GameMenu()
    {
        musician.SetMood(Mood.Tender, 0.5f);
        musician.Play();
    }

    void GameStart()
    {
        musician.SetMood(Mood.Happy, 0.5f);
    }

    void GameOver()
    {
        musician.SetMood(Mood.Angry, 0.5f);
    }
}