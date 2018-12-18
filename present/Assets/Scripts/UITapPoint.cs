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

    public Transform holder;
    public GameObject MoodColor;

    public Sprite comboLink;

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
        foreach (var item in note.moodScores)
        {
            
            lis.Add(item.Key.ToString() + ": " + item.Value.ToString());
            GameObject a = Instantiate<GameObject>(MoodColor, holder.transform);
            a.GetComponent<MoodColorScript>().SetColor(item.Key.ToString(), item.Value);
        }
        //txt.text = DMUtils.StringArray(lis.ToArray());

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

            if (currentTime > myNote.time + GameController.TAP_NODE_TIME_WINDOW * 1.15f)
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
