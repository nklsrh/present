using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITapPoint : MonoBehaviour
{
    //public Text txt;
    public const float DISTANCE_PER_SECOND = 500;

    private Note myNote;
    private RectTransform rect;

    internal void SetupNote(float currentTime, Note note)
    {
        myNote = note;
        rect = GetComponent<RectTransform>();

        SetTime(currentTime);
    }

    public void SetTime(float currentTime)
    {
        if (myNote != null && rect != null)
        {
            float secondsFromNow = myNote.time - currentTime;

            rect.anchoredPosition = new Vector2(secondsFromNow * DISTANCE_PER_SECOND, rect.anchoredPosition.y);
        }
    }
}
