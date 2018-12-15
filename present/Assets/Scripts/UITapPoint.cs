using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITapPoint : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public const float DISTANCE_PER_SECOND = 200;

    private Note myNote;
    private RectTransform rect;

    public Note Note
    {
        get
        {
            return myNote;
        }
    }

    internal void SetupNote(float currentTime, Note note)
    {
        myNote = note;
        rect = GetComponent<RectTransform>();
        List<string> lis = new List<string>();
        foreach (var item in note.moods)
        {
            lis.Add(item.ToString());
        }
        txt.text = DMUtils.StringArray(lis.ToArray()) + " " + note.score;

        SetTime(currentTime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentTime"></param>
    /// <returns>if note expires and runs out of time, return true</returns>
    public bool SetTime(float currentTime)
    {
        if (myNote != null && rect != null)
        {
            float secondsFromNow = myNote.time - currentTime;

            rect.anchoredPosition = new Vector2(secondsFromNow * DISTANCE_PER_SECOND, rect.anchoredPosition.y);

            if (currentTime > myNote.time + GameController.TAP_NODE_TIME_WINDOW)
            {
                Hide();
                return true;
            }
        }
        return false;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
