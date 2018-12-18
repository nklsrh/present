using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIScoring : MonoBehaviour {

    public TextMeshProUGUI txtScoreA;
    public TextMeshProUGUI txtScoreB;
    public TextMeshProUGUI txtScoreC;
    public TextMeshProUGUI txtAnger;

    Dictionary<Note.Mood, int> required = new Dictionary<Note.Mood, int>();
    Dictionary<Note.Mood, int> score = new Dictionary<Note.Mood, int>();

    int anger;
    int angerMax;

    public Slider SliderA, SliderB, SliderC, SliderAnger;

    public void SetScore(Note.Mood mood, int score, int required)
    {
        this.required[mood] = required;

        SetScore(mood, score);
    }

    public void SetAngerMax(int value)
    {
        angerMax = value;
    }

    public void SetAnger(int newValue)
    {
        anger = newValue;
        txtAnger.text = anger + "/" + angerMax;

        if (SliderAnger != null && angerMax > 0)
        {
            SliderAnger.value = SliderAnger.minValue + (SliderAnger.maxValue - SliderAnger.minValue) * (anger / angerMax);
        }
    }

    internal void SetScore(Note.Mood mood, int score)
    {
        this.score[mood] = score;

        TextMeshProUGUI t = null;
        t = (mood == Note.Mood.A ? txtScoreA : mood == Note.Mood.B ? txtScoreB : mood == Note.Mood.C ? txtScoreC : null);
        if (t != null)
        {
            t.text = "[" + mood + "] " + this.score[mood] + "/" + required[mood];

            if (t == txtScoreA && SliderA != null)
            {
                SliderA.value = score;
            } else if (t == txtScoreB && SliderB != null)
            {
                SliderB.value = score;
            }
            else if (t == txtScoreC && SliderC != null)
            {
                SliderC.value = score;
            }
        }
        //SliderA.value = no;
        //SliderB.value = txtScoreB;
       // SliderC.value = txtScoreC;

    }
}
