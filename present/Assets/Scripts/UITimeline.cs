﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimeline : MonoBehaviour {

    [Header("Fill these up")]
    public UITapPoint tapPoint;

    [Header("Ignore these")]
    public List<UITapPoint> tapPoints;

    public Action<UITapPoint> onMissedNote;

    public void Setup (Song song)
    {
        tapPoints.Clear();

        var songNotes = song.notes.ToArray();
        DMUtils.BuildList<UITapPoint, Note>(OnBuildTapPoint, songNotes, tapPoint.gameObject, tapPoint.transform.parent);
	}

    private void OnBuildTapPoint(UITapPoint ui, Note note)
    {
        ui.SetupNote(0, note);

        tapPoints.Add(ui);
    }

    public void UpdateLogic (float currentTime)
    {
        for (int i = 0; i < tapPoints.Count; i++)
        {
            if (tapPoints[i].gameObject.activeSelf)
            {
                if (tapPoints[i].SetTime(currentTime))
                {
                    if (onMissedNote != null)
                    {
                        onMissedNote.Invoke(tapPoints[i]);
                    }
                }
            }
            else
            {
                tapPoints.Remove(tapPoints[i]);
            }
        }
	}

    internal void HideLatestNode()
    {
        tapPoints[0].gameObject.SetActive(false);
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
    public List<MoodValue> moodChanges;
}

[System.Serializable]
public class DrawCard : Card
{
    public int cardsToDraw;
}

[System.Serializable]
public class Card
{
    public string name;
}

[System.Serializable]
public class MoodValue
{
    public Note.Mood mood;
    public int value;
}

[System.Serializable]
public class Song
{
    public List<Note> notes;
    
    internal Note GetNote(float time)
    {
        for (int i = 0; i < notes.Count; i++)
        {
            if (notes[i].time < time + GameController.TAP_NODE_TIME_WINDOW &&
                notes[i].time > time - GameController.TAP_NODE_TIME_WINDOW)
            {
                return notes[i];
            }
        }

        return null;
    }
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

    public List<Mood> moods = new List<Mood>();
    public bool isComboNote = false;
    public int score;

    public float time;
}

