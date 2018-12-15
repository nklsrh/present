﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIScoring : MonoBehaviour {

    public TextMeshProUGUI txtScoreA;
    public TextMeshProUGUI txtScoreB;
    public TextMeshProUGUI txtScoreC;

    Dictionary<Note.Mood, int> required = new Dictionary<Note.Mood, int>();
    Dictionary<Note.Mood, int> score = new Dictionary<Note.Mood, int>();

    public void SetScore(Note.Mood mood, int score, int required)
    {
        this.required[mood] = required;

        SetScore(mood, score);
    }

    internal void SetScore(Note.Mood mood, int score)
    {
        this.score[mood] = score;

        TextMeshProUGUI t = null;
        t = (mood == Note.Mood.A ? txtScoreA : mood == Note.Mood.B ? txtScoreB : mood == Note.Mood.C ? txtScoreC : null);
        if (t != null)
        {
            t.text = "[" + mood + "] " + this.score[mood] + "/" + required[mood];
        }
    }
}