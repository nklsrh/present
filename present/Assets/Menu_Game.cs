using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Game : MonoBehaviour
{
    public List<Animation> animationsABC;
    public List<MoodColorScript> moods;

    private Dictionary<Note.Mood, Animation> animationDictonary = new Dictionary<Note.Mood, Animation>();

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

        animationDictonary[(Note.Mood)moodMinusBlank].Play();
        moods[moodMinusBlank].SetColor(mood.ToString(), value);
    }
}
