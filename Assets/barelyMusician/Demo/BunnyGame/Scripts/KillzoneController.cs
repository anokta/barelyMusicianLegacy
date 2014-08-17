using UnityEngine;
using System.Collections;
using BarelyAPI;

public class KillzoneController : MonoBehaviour {

    Musician musician;

    SpriteRenderer killzone;
    Color killzoneColorCurrent, killzoneColorTarget;

    public Vector2 direction;
    Vector3 position;

    void Start () {
        killzone = GetComponent<SpriteRenderer>();

        musician = FindObjectOfType<Musician>();
        musician.Sequencer.AddBeatListener(OnNextBeat);

        position = transform.position;
        transform.localScale = Vector3.Scale(GameWorld.ScreenBounds, Vector2.one - (1.0f - 1.0f / musician.Sequencer.BeatCount) * new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y)));
        killzoneColorCurrent = killzoneColorTarget = Color.clear;

        musician.Ensemble.TogglePeformer("Octave", true);
	}
	
    void Update () {
        transform.position = position;
        if (Mathf.Abs(position.x) > GameWorld.ScreenBounds.x / 2.0f || Mathf.Abs(position.y) > GameWorld.ScreenBounds.y / 2.0f)
        {
            musician.Ensemble.TogglePeformer("Octave", false);
            GameObject.Destroy(gameObject);
            return;
        }

        if (killzoneColorCurrent != killzoneColorTarget)
        {
            killzoneColorCurrent = Color.Lerp(killzoneColorCurrent, killzoneColorTarget, 2.0f * Time.deltaTime);
            killzone.color = killzoneColorCurrent;
        }
	}

    void OnNextBeat(Sequencer sequencer)
    {
        position += Vector3.Scale(direction, GameWorld.ScreenBounds) / sequencer.BeatCount;
        killzoneColorCurrent = new Color(1.0f, 0.1f, 0.1f);
    }
}
