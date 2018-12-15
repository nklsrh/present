using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoring : MonoBehaviour {

    public TextMeshProUGUI txtScoreA;
    public TextMeshProUGUI txtScoreB;
    public TextMeshProUGUI txtScoreC;

    public void SetScore(Note.Mood mood, int score, int required)
    {
        TextMeshProUGUI t = null;
        t = (mood == Note.Mood.A ? txtScoreA : mood == Note.Mood.B ? txtScoreB : mood == Note.Mood.C ? txtScoreC : null);
        
        if (t != null)
        {
            t.text = "[" + mood + "] " + score + "/" + required;
        }
    }
}
