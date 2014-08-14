using UnityEngine;
using System.Collections;
using BarelyAPI;

public class BunnyController : MonoBehaviour
{
    Musician musician;
    Animator animator;

    float speed;

    Vector2 direction = Vector2.zero;

    void Start()
    {
        musician = FindObjectOfType<Musician>();
        animator = GetComponent<Animator>();

        Vector3 screen = Camera.main.WorldToScreenPoint(transform.position);

        musician.Ensemble.TogglePeformer("Bunny", false);
    }

    void Update()
    {
        speed = musician.Sequencer.Tempo / 60.0f;
        animator.speed = speed;

        CheckBoundaries();

        transform.Translate(direction * Time.deltaTime * speed * GameWorld.ScreenBounds.x / (musician.Sequencer.BeatCount));

        int x = (int)Input.GetAxisRaw("Horizontal");
        int y = (int)Input.GetAxisRaw("Vertical");
        direction = new Vector2(x, y);

        SetAnimationState(direction);
    }

    void CheckBoundaries()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x > Screen.width)
        {
            transform.Translate(Vector3.left * GameWorld.ScreenBounds.x);
        }
        if (screenPosition.x < 0.0f)
        {
            transform.Translate(Vector3.right * GameWorld.ScreenBounds.x);
        }
        if (screenPosition.y > Screen.height)
        {
            transform.Translate(Vector3.down * GameWorld.ScreenBounds.y);
        }
        if (screenPosition.y < 0.0f)
        {
            transform.Translate(Vector3.up *  GameWorld.ScreenBounds.y);
        }
    }

    void SetAnimationState(Vector2 directionVector)
    {
        string direction = null;

        if (directionVector == new Vector2(0, -1))
        {
            direction = "Down";
        }
        else if (directionVector == new Vector2(0, 1))
        {
            direction = "Up";
        }
        else if (directionVector == new Vector2(1, 0))
        {
            direction = "Right";
        }
        else if (directionVector == new Vector2(-1, 0))
        {
            direction = "Left";
        }
        else if (directionVector == Vector2.zero)
        {
            animator.SetInteger("State", 0);
            musician.Ensemble.TogglePeformer("Bunny", false);
        }

        if (direction != null)
        {
            animator.SetInteger("State", 1);
            animator.CrossFade("Bunny_Running_" + direction, 0.0f);
            musician.Ensemble.TogglePeformer("Bunny", true);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        GameEventManager.TriggerGameOver();
    }
}