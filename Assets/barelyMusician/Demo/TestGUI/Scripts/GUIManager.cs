using UnityEngine;
using System.Collections;
using BarelyAPI;

public class GUIManager : MonoBehaviour
{
    public GUISkin guiSkin;

    public GUIText timeSignutare;
    //private string timeSigText;

    public Musician musician;

    // Use this for initialization
    void Start()
    {
    }

    void OnGUI()
    {
        GUI.skin = guiSkin;

        //timeSignutare.text = timeSigText;
        GUILayout.BeginArea(new Rect(Screen.width * 0.25f, Screen.height * 0.2f, Screen.width * 0.5f, Screen.height * 0.75f));

        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();

        //GUILayout.BeginHorizontal();
        //GUILayout.Box("BPM: " + Sequencer.Tempo.ToString());
        //Sequencer.Tempo = (int)GUILayout.HorizontalSlider(Sequencer.Tempo, 72, 220);
        //GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Play"))
        {
            musician.Play();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Pause"))
        {
            musician.Pause();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Stop"))
        {
            musician.Stop();
        }

        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("EXCITING"))
        {
            musician.SetMood(Mood.Exciting, 0.5f);
        }
        if (GUILayout.Button("HAPPY"))
        {
            musician.SetMood(Mood.Happy, 0.5f);
        }
        if (GUILayout.Button("TENDER"))
        {
            musician.SetMood(Mood.Tender, 0.5f);
        }
        if (GUILayout.Button("DEPRESSED"))
        {
            musician.SetMood(Mood.Depressed, 0.5f);
        }
        if (GUILayout.Button("SAD"))
        {
            musician.SetMood(Mood.Sad, 0.5f);
        }
        if (GUILayout.Button("ANGRY"))
        {
            musician.SetMood(Mood.Angry, 0.5f);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Box("Energy");
        float energy = GUILayout.HorizontalSlider(musician.Energy, 0.0f, 1.0f);
        if (energy != musician.Energy)
            musician.SetEnergy(energy);
        GUILayout.Box("Stress");
        float stress = GUILayout.HorizontalSlider(musician.Stress, 0.0f, 1.0f);
        if (stress != musician.Stress)
            musician.SetStress(stress);
        GUILayout.EndVertical();
        
        //GUILayout.BeginVertical();
        //ensemble.PrintValues();
        //GUILayout.EndVertical();

        //GUILayout.EndHorizontal();
        //

        GUILayout.FlexibleSpace();

        GUILayout.EndVertical();

        GUILayout.EndArea();
    }
}