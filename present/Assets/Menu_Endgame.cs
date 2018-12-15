using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Endgame : MonoBehaviour {

    public Button btnContinue;
    public Menu_LevelSelect levelSelect;
    public GameController game;

    public UIScoring scoring;

    // Use this for initialization
    void Start ()
    {
        btnContinue.onClick.AddListener(() =>
        {
            ExitToMenu();
        });
    }

    internal void Show()
    {
        scoring.SetScore(Note.Mood.A, 0, game.level.scoreA);
        scoring.SetScore(Note.Mood.B, 0, game.level.scoreB);
        scoring.SetScore(Note.Mood.C, 0, game.level.scoreC);

        foreach (var item in game.moodValues)
        {
            scoring.SetScore(item.mood, item.value);
        }

        gameObject.SetActive(true);
    }

    private void ExitToMenu()
    {
        levelSelect.gameObject.SetActive(true);
        levelSelect.Refresh();

        gameObject.SetActive(false);
    }
}
