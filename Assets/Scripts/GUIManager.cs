using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{

    public GUIText timeSignutare;
    private string timeSigText;

    // Use this for initialization
    void Start()
    {
        AudioEventManager.OnNextBeat += OnNextBeat;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnGUI()
    {
        timeSignutare.text = timeSigText;
        GUILayout.BeginArea(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.5f));

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();

        if (GUILayout.Button("Play"))
        {
            MainClock.Play();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Pause"))
        {
            MainClock.Pause();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Stop"))
        {
            MainClock.Stop();
        }

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();


        //GUILayout.HorizontalSlider(5, 40, 300);


        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    void OnNextBeat(int beat)
    {
        timeSigText = beat.ToString();
    }
}
