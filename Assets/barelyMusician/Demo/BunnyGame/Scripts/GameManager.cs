using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GameManager : MonoBehaviour {

    public Musician musician;

    void Awake()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
	}

    void Start()
    {
        GameEventManager.TriggerGameMenu();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameEventManager.TriggerGameStart();
        } 
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameEventManager.TriggerGameOver();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameEventManager.TriggerGameMenu();
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
