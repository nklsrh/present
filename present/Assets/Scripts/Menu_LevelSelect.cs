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

    public UICard cardDeck;
    public UICard cardInventory;

    DLevel selectedLevel;
    List<DCard> selectedCards = new List<DCard>();
    List<DCard> inventoryCards = new List<DCard>();

    private void Start()
    {
        inventoryCards = new List<DCard>();
        inventoryCards.AddRange(Data.card.ToArray());

        selectedCards = new List<DCard>()
        {
            Data.card.card_mood_21
            ,    Data.card.card_mood_42
            ,    Data.card.card_mood_63
            ,    Data.card.card_combo_3
            ,    Data.card.card_combo_6
            ,    Data.card.card_combo_9
            ,    Data.card.card_draw_2
            ,    Data.card.card_mood_16
            ,    Data.card.card_mood_4
            ,    Data.card.card_mood_32
            ,    Data.card.card_mood_54
            ,    Data.card.card_mood_40
            ,    Data.card.card_mood_36
            ,    Data.card.card_mood_37
            ,    Data.card.card_mood_62,   Data.card.card_mood_21
            ,    Data.card.card_mood_42
            ,    Data.card.card_mood_63
            ,    Data.card.card_combo_3
            ,    Data.card.card_combo_6
            ,    Data.card.card_combo_9
            ,    Data.card.card_draw_2
            ,    Data.card.card_mood_16
            ,    Data.card.card_mood_4
            ,    Data.card.card_mood_32
            ,    Data.card.card_mood_54
            ,    Data.card.card_mood_40
            ,    Data.card.card_mood_36
            ,    Data.card.card_mood_37
            ,    Data.card.card_mood_62,
        };

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

        BuildDeck();
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

        List<Card> cards = new List<Card>();
        for (int i = 0; i < selectedCards.Count; i++)
        {
            cards.Add(selectedCards[i].GetCard());
        }
        gameController.Setup(selectedLevel, cards);
    }

    public void BuildDeck()
    {
        DMUtils.BuildList<UICard, DCard>(OnBuildCardInventory, inventoryCards.ToArray(), cardInventory.gameObject, cardInventory.transform.parent);
        DMUtils.BuildList<UICard, DCard>(OnBuildCardDeck, selectedCards.ToArray(), cardDeck.gameObject, cardDeck.transform.parent);
    }

    private void OnBuildCardInventory(UICard arg1, DCard arg2)
    {
        arg1.SetCard(arg2);
        arg1.SetAction(OnSelectCardInventory);
    }

    private void OnBuildCardDeck(UICard arg1, DCard arg2)
    {
        arg1.SetCard(arg2);
        arg1.SetAction(OnSelectCardDeck);
    }

    private void OnSelectCardInventory(DCard obj)
    {
        selectedCards.Add(obj);
        inventoryCards.Remove(obj);

        BuildDeck();
    }

    private void OnSelectCardDeck(DCard obj)
    {
        inventoryCards.Add(obj);
        selectedCards.Remove(obj);

        BuildDeck();
    }
}
