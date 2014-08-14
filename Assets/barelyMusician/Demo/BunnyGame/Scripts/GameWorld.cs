using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GameWorld : MonoBehaviour
{

    public GameObject bunnyPrefab;
    public GameObject backgroundPrefab, killzonePrefab;

    BunnyController bunny;
    SpriteRenderer background;

    Color backgroundTarget;

    Vector2 screenSize;
    public static Vector2 ScreenBounds
    {
        get { return _instance.screenSize; }
    }

    Musician musician;
    int beatCount, currentBeat;
    bool newSection;

    static GameWorld _instance;

    void Awake()
    {
        _instance = this;

        screenSize = 2.0f * Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -Camera.main.transform.position.z));
    }

    void Start()
    {
        GameEventManager.GameMenu += GameMenu;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;

        musician = FindObjectOfType<Musician>();
        musician.Sequencer.AddSectionListener(OnNextSection);
        musician.Sequencer.AddBarListener(OnNextBar);
        musician.Sequencer.AddBeatListener(OnNextBeat);
        musician.Sequencer.AddPulseListener(OnNextPulse);

        beatCount = musician.Sequencer.BeatCount;

        background = (GameObject.Instantiate(backgroundPrefab) as GameObject).GetComponent<SpriteRenderer>();
        background.transform.localScale = screenSize;
    }

    void Update()
    {
        switch (GameEventManager.CurrentState)
        {
            case GameEventManager.GameState.Running:
                if (newSection)
                {
                    newSection = false;
                    Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                    direction.x = direction.x < 0.75f ? (direction.x > -0.75f ? (direction.x > 0.0f ? 0.75f : -0.75f) : direction.x) : direction.x;
                    direction.y = direction.y < 0.75f ? (direction.y > -0.75f ? (direction.y > 0.0f ? 0.75f : -0.75f) : direction.y) : direction.y;
                    KillzoneController killzone =
                        (GameObject.Instantiate(killzonePrefab, Vector2.Scale(direction, screenSize) * (currentBeat - (beatCount - 1) / 2.0f) / beatCount, Quaternion.identity) as GameObject).GetComponent<KillzoneController>();
                    killzone.direction = direction;
                }
                break;
        }

        if (GameEventManager.CurrentState != GameEventManager.GameState.InMenu)
        {
            if (backgroundTarget != background.color)
                background.color = Color.Lerp(background.color, backgroundTarget, 2.0f * Time.deltaTime);
        }
    }

    void GameMenu()
    {
        backgroundTarget = Color.white;
    }

    void GameStart()
    {
        newSection = false;
        backgroundTarget = Color.white;
        bunny = (GameObject.Instantiate(bunnyPrefab) as GameObject).GetComponent<BunnyController>();
    }

    void GameOver()
    {
        backgroundTarget = Color.black;
        GameObject.Destroy(bunny.gameObject);
    }

    void OnNextSection(Sequencer sequencer)
    {
        newSection = true;
        //backgroundTarget = new Color(RandomNumber.NextFloat(), RandomNumber.NextFloat(), RandomNumber.NextFloat());
    }

    void OnNextBar(Sequencer sequencer)
    {
    }

    void OnNextBeat(Sequencer sequencer)
    {
        currentBeat = sequencer.CurrentBeat;
    }

    void OnNextPulse(Sequencer sequencer)
    {
    }
}