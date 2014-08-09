using UnityEngine;
using System.Collections;
using BarelyAPI;

public class BunnyController : MonoBehaviour {

    public Musician musician;

    Animator animator;
    float speed;

    float boundX;

    void Start()
    {
        animator = GetComponent<Animator>();

        Vector3 screen = Camera.main.WorldToScreenPoint(transform.position);
        boundX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Camera.main.transform.position.y - screen.y, Camera.main.transform.position.z - screen.z)).x;
        transform.position = Vector3.left * boundX * 0.75f;

        musician.Sequencer.AddBarListener(OnNextBar);
    }
	
    void Update () 
    {
        speed = musician.IsPlaying ? musician.Sequencer.Tempo / 60.0f : 0.0f;
        animator.speed = speed;

        if (Camera.main.WorldToScreenPoint(transform.position).x > Screen.width)
        {
            transform.Translate(Vector3.left * 2.0f * boundX);
        }
        transform.Translate(Vector3.right * Time.deltaTime * speed * boundX / 4.0f);

        if (speed == 0.0f)
        {
            transform.position = Vector3.left * boundX * 0.75f;
        }
	}

    void OnNextBar(SequencerState state)
    {
    }
}
