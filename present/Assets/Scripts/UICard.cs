using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICard : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public Button button;

    private Card card;

    internal void SetCard(Card card)
    {
        this.card = card;

        string title = "???";
        string subtitle = "";
        if (card is MoodCard)
        {
            title = "Mood";
            string ss = "";
            var c = card as MoodCard;
            for (int i = 0; i < c.moodChanges.Count; i++)
            {
                ss += "<size=93>" + c.moodChanges[i].mood.ToString() + "<sup>" + c.moodChanges[i].value + "</sup></size>\n";
            }
            subtitle = ss;
        }
        if (card is DrawCard)
        {
            title = "Draw";
            var c = card as DrawCard;
            subtitle = c.cardsToDraw + "x";
        }
        if (card is ComboCard)
        {
            title = "Combo";
            var c = card as ComboCard;
            subtitle = "<size=93>" + c.mood.ToString() + "<sup>" + (c.multiplier * 100).ToString("###0") + "%</sup></size>\n";
        }
        txt.text = title + "\n" + subtitle;
    }

    internal void SetAction(Action<Card> onPlayCard)
    {
        if (onPlayCard != null && button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=> { onPlayCard.Invoke(card); });
        }
    }
}
