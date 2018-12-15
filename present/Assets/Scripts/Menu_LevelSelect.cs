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

        selectedCards = new List<DCard>();

        for (int i = 0; i < Data.startingDeck.Length; i++)
        {
            var card = Data.card[Data.startingDeck[i].id];
            selectedCards.Add(card);
            inventoryCards.Remove(card);
        }

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

            // limit number of selected cards to max size specified by level
            for (int i = selectedCards.Count - 1; i >= 0; i--)
            {
                if (i > selectedLevel.deckSize - 1)
                {
                    var card = selectedCards[i];
                    selectedCards.Remove(card);
                    inventoryCards.Add(card);
                }
            }

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
        if (selectedCards.Count < selectedLevel.deckSize)
        {
            selectedCards.Add(obj);
            inventoryCards.Remove(obj);

            BuildDeck();
        }
    }

    private void OnSelectCardDeck(DCard obj)
    {
        inventoryCards.Add(obj);
        selectedCards.Remove(obj);

        BuildDeck();
    }
}
