using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimeline : MonoBehaviour {

    [Header("Fill these up")]
    public UITapPoint tapPoint;
    public Song song;

    [Header("Ignore these")]
    public List<UITapPoint> tapPoints;

    public float currentTime;

    void Start ()
    {
        tapPoints.Clear();

        var songNotes = song.notes.ToArray();
        DMUtils.BuildList<UITapPoint, Note>(OnBuildTapPoint, songNotes, tapPoint.gameObject, tapPoint.transform.parent);

        //for (int i = 0; i < songNotes.Length; i++)
        {
        }
	}

    private void OnBuildTapPoint(UITapPoint ui, Note note)
    {
        ui.SetupNote(currentTime, note);

        tapPoints.Add(ui);
    }

    void Update ()
    {
        for (int i = 0; i < tapPoints.Count; i++)
        {
            tapPoints[i].SetTime(currentTime);
        }

        currentTime += Time.deltaTime;
	}
}

[System.Serializable]
public class ComboCard : Card
{
    public float multiplier;
    public Note.Mood mood;
}

[System.Serializable]
public class MoodCard : Card
{
    public List<MoodChange> moodChanges;
}

[System.Serializable]
public class DrawCard : Card
{
    public int cardsToDraw;
}

[System.Serializable]
public class Card
{

}

public struct MoodChange
{
    public Note.Mood mood;
    public int value;
}

[System.Serializable]
public class Song
{
    public List<Note> notes;
}

[System.Serializable]
public class Note
{
    public enum Mood
    {
        Blank = 0,
        A = 1,
        B = 2,
        C = 3,
    }

    public Mood mood;

    public float time;
}

