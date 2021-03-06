﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICard : MonoBehaviour
{
    //public TextMeshProUGUI txt;
    public Button button;

    private Card card;
    private DCard dcard;

    public GameObject colorHolder;
    public GameObject moodColor;

    public GameObject drawCardHolder;
    public TextMeshProUGUI drawText;

    public GameObject comboCardHolder;
    public RawImage comboColor;
    public TextMeshProUGUI comboText;



    internal void SetCard(DCard card)
    {
        dcard = card;
        SetCard(card.GetCard());
    }

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
               // ss += "<size=93>" + c.moodChanges[i].mood.ToString() + "<sup>" + c.moodChanges[i].value + "</sup></size>\n";
                GameObject a = Instantiate<GameObject>(moodColor, colorHolder.transform);
                a.GetComponent<MoodColorScript>().SetColor(c.moodChanges[i].mood.ToString(), c.moodChanges[i].value);
                
            }
            subtitle = ss;
        }
        if (card is DrawCard)
        {
            title = "Draw";
            var c = card as DrawCard;
            subtitle = c.cardsToDraw + "x";

            drawCardHolder.SetActive(true);
            drawText.text = "<size=42>x" + "</size>" + c.cardsToDraw;
        }
        if (card is ComboCard)
        {
            title = "Combo";
            var c = card as ComboCard;
            subtitle = "<size=93>" + c.mood.ToString() + "<sup>" + (c.multiplier * 100).ToString("###0") + "%</sup></size>\n";

            comboCardHolder.SetActive(true);
            if (c.mood.ToString() == "A")
            {
                comboColor.color = Color.green;
            } else if (c.mood.ToString() == "B")
            {
                comboColor.color = Color.blue;
            }
            else if (c.mood.ToString() == "C")
            {
                comboColor.color = Color.magenta;
            }
            comboText.text = "<size=42>x" + "</size>" + c.multiplier;
        }
        //txt.text = title + "\n" + subtitle;
    }

    internal void SetAction(Action<Card> onPlayCard)
    {
        if (onPlayCard != null && button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(()=> { onPlayCard.Invoke(card); });
        }
    }

    internal void SetAction(Action<DCard> onPlayCard)
    {
        if (onPlayCard != null && button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { onPlayCard.Invoke(dcard); });
        }
    }

}
