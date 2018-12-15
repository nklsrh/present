using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_LevelSelect : MonoBehaviour {

    public UILevelButton btnLevel;
    public GameObject menu_Game;
    public GameController gameController;
    public Button btnLevels;
    public Button btnPlay;
    public Animation animations;

    DLevel selectedLevel;

    private void Start()
    {
        Refresh();

        gameObject.SetActive(true);
    }

    internal void Refresh()
    {
        DMUtils.BuildList<UILevelButton, DLevel>(OnBuildButton, Data.level.ToArray(), btnLevel.gameObject, btnLevel.transform.parent);

        btnLevels.onClick.RemoveAllListeners();
        btnLevels.onClick.AddListener(() =>
        {
            animations.Play("menu_slide_levels");
        });
        btnPlay.onClick.RemoveAllListeners();
        btnPlay.onClick.AddListener(() =>
        {
            StartLevel();
        });

        animations.Play("menu_slide_levels");
    }

    private void OnBuildButton(UILevelButton levelBtn, DLevel level)
    {
        levelBtn.SetLevel(level, ()=>
        {
            selectedLevel = level;
            animations.Play("menu_slide_cards");
        });
    }

    public void StartLevel()
    {
        this.gameObject.SetActive(false);
        menu_Game.gameObject.SetActive(true);

        gameController.Setup(selectedLevel);
    }
}
