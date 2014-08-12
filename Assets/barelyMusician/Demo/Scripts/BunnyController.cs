using UnityEngine;
using System.Collections;
using BarelyAPI;

public class BunnyController : MonoBehaviour
{

    public Musician musician;

    Animator animator;
    float speed;

    float boundX;
    Vector2 direction = Vector2.zero;

    void Start()
    {
        animator = GetComponent<Animator>();

        Vector3 screen = Camera.main.WorldToScreenPoint(transform.position);
        boundX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Camera.main.transform.position.y - screen.y, Camera.main.transform.position.z - screen.z)).x;
    }

    void Update()
    {
        speed = musician.IsPlaying ? musician.Sequencer.Tempo / 60.0f : 0.0f;
        animator.speed = direction.magnitude * speed;

        if (Camera.main.WorldToScreenPoint(transform.position).x > Screen.width)
        {
            transform.Translate(Vector3.left * 2.0f * boundX);
        }
        transform.Translate(direction * Time.deltaTime * speed * boundX / (musician.Sequencer.BeatCount / 2.0f));

        if (speed == 0.0f)
        {
            transform.position = new Vector3(-boundX * (1.0f - 2.0f / musician.Sequencer.BeatCount), transform.position.y, transform.position.z);
        }

        int x = (int)Input.GetAxisRaw("Horizontal");
        int y = (int)Input.GetAxisRaw("Vertical");
        direction = new Vector2(x, y);
        //if (direction.magnitude == 0.0f) musician.Ensemble.MutePerformer("Naber");
        //else musician.Ensemble.UnmutePerformer("Naber");
        SetAnimationState(direction);
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

        if (direction != null)
        {
            animator.CrossFade("Bunny_Running_" + direction, 0.0f);
        }
    }

}