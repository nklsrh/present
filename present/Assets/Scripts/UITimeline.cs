using System;
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
        onMissedNote = null;

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

    internal bool IsLevelComplete()
    {
        return tapPoints.Count == 0;
    }
}

[System.Serializable]
public class ComboCard : Card
{
    public int multiplier;
    public Note.Mood mood;
}

[System.Serializable]
public class MoodCard : Card
{
    public List<MoodValue> moodChanges;

    public int GetMoodValue(Note.Mood mood)
    {
        var foundIndex = moodChanges.FindIndex(r => r.mood == mood);
        if (foundIndex >= 0)
        {
            return moodChanges[foundIndex].value;
        }
        return -1;
    }
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

    public Dictionary<Mood, int> moodScores = new Dictionary<Mood, int>();
    public bool isComboNote = false;

    public float time;

    internal Mood GetFirstMatchingMood(Note comboStartNote)
    {
        Mood m = Mood.Blank;

        foreach (var item in moodScores.Keys)
        {
            if (comboStartNote.moodScores.ContainsKey(item))
            {
                return item;
            }
        }

        return m;
    }
}

