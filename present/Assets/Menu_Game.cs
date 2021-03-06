﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Game : MonoBehaviour
{
    public List<Animation> animationsABC;
    public List<MoodColorScript> moods;

    private Dictionary<Note.Mood, Animation> animationDictonary = new Dictionary<Note.Mood, Animation>();

    public Animation anger;
    public TMPro.TextMeshProUGUI txtAnger;

    public Animation angerFail;

    public Animation levelFinsh;

    void Start ()
    {
        for (int i = 0; i < animationsABC.Count; i++)
        {
            animationDictonary.Add((Note.Mood)i, animationsABC[i]);
        }
	}

    internal void AnimateScore(Note.Mood mood, int value)
    {
        int moodMinusBlank = (int)mood - 1;

        Note.Mood moodSelcted = (Note.Mood)moodMinusBlank;

        animationDictonary[moodSelcted].Stop();
        animationDictonary[moodSelcted].Rewind();
        animationDictonary[moodSelcted].Play();

        moods[moodMinusBlank].SetColor(mood.ToString(), value);
    }

    internal void AnimateAnger(int angerAmount)
    {
        txtAnger.text = angerAmount.ToString("00");
        anger.gameObject.SetActive(true);
        anger.Play();
    }   

    internal void AnimateAngerFail()
    {
        angerFail.gameObject.SetActive(true);
        angerFail.Play();
    }

    public void AnimateLevelFinish()
    {
        levelFinsh.gameObject.SetActive(true);
        levelFinsh.Play();
    }

    internal void Reset()
    {
        levelFinsh.gameObject.SetActive(false);
        levelFinsh.Stop();
        levelFinsh.Rewind();
        anger.gameObject.SetActive(false);
        anger.Stop();
        anger.Rewind();
        angerFail.gameObject.SetActive(false);
        angerFail.Stop();
        angerFail.Rewind();
    }
}
