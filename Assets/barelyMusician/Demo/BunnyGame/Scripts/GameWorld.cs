// ----------------------------------------------------------------------
//   Adaptive music composition engine implementation for interactive systems.
//
//     Copyright 2014 Alper Gungormusler. All rights reserved.
//
// ------------------------------------------------------------------------

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
    int initialTempo;

    static GameWorld _instance;

    void Awake()
    {
        _instance = this;

        screenSize = 2.0f * Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -Camera.main.transform.position.z));

        GameEventManager.GameMenu += GameMenu;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
    }

    void Start()
    {
        musician = FindObjectOfType<Musician>();
        musician.Sequencer.AddSectionListener(OnNextSection);
        musician.Sequencer.AddBarListener(OnNextBar);
        musician.Sequencer.AddBeatListener(OnNextBeat);
        musician.Sequencer.AddPulseListener(OnNextPulse);

        initialTempo = musician.Tempo;
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
                    direction.x = Mathf.Round(direction.x);//direction.x < 0.75f ? (direction.x > -0.75f ? (direction.x > 0.0f ? 0.75f : -0.75f) : direction.x) : direction.x);
                    direction.y = Mathf.Round(direction.y);//direction.y < 0.75f ? (direction.y > -0.75f ? (direction.y > 0.0f ? 0.75f : -0.75f) : direction.y) : direction.y);
                    if(direction == Vector2.zero) direction = Vector2.right;
                    KillzoneController killzone =
                        (GameObject.Instantiate(killzonePrefab, Vector2.Scale(direction, screenSize) * (currentBeat - (beatCount - 1) / 2.0f) / beatCount, Quaternion.identity) as GameObject).GetComponent<KillzoneController>();
                    killzone.direction = direction;
                }
                break;
        }

        if (backgroundTarget != background.color)
            background.color = Color.Lerp(background.color, backgroundTarget, 2.0f * Time.deltaTime);
    }

    void GameMenu()
    {
        if(musician != null)
            musician.Tempo = initialTempo;
        backgroundTarget = new Color(1.0f, 1.0f, 1.0f, 0.29f);
    }

    void GameStart()
    {
        musician.Tempo = initialTempo;
        KillzoneController[] killzones = FindObjectsOfType<KillzoneController>();
        foreach (KillzoneController killzone in killzones)
            GameObject.Destroy(killzone.gameObject);

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
        if(musician.Energy == 1.0f || musician.Ensemble.CurrentSection != SectionType.INTRO || musician.Ensemble.CurrentSection != SectionType.OUTRO)
            newSection = true;
        //backgroundTarget = new Color(RandomNumber.NextFloat(), RandomNumber.NextFloat(), RandomNumber.NextFloat());

        if (musician.Energy == 1.0f) musician.Tempo = Mathf.Min(220, musician.Tempo + 1);
    }

    void OnNextBar(Sequencer sequencer)
    {
        musician.SetEnergy(musician.Energy + 0.05f, 0.25f);
        if ((musician.Energy == 1.0f && musician.Ensemble.CurrentSection != SectionType.INTRO && musician.Ensemble.CurrentSection != SectionType.OUTRO) || (musician.Energy > 0.55f && musician.Ensemble.CurrentSection == SectionType.CHORUS))
            newSection = true;
    }

    void OnNextBeat(Sequencer sequencer)
    {
        currentBeat = sequencer.CurrentBeat;
    }

    void OnNextPulse(Sequencer sequencer)
    {
    }
}