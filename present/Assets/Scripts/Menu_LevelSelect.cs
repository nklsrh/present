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
    public Button btnAll;
    public Button btnMoodA;
    public Button btnMoodB;
    public Button btnMoodC;
    public Button btnCombo;
    public Button btnAction;


    public Animation animations;

    public UICard cardDeck;
    public UICard cardInventory;

    DLevel selectedLevel;
    List<DCard> selectedCards = new List<DCard>();
    List<DCard> inventoryCards = new List<DCard>();

    public enum eCardType
    {
        All,
        moodA,
        moodB,
        moodC,
        Combo,
        Action
    }


    public eCardType currentCategory = eCardType.All;

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

        btnMoodA.onClick.RemoveAllListeners();
        btnMoodA.onClick.AddListener(() =>
        {
            currentCategory = eCardType.moodA;
            BuildDeck();
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
        

        List<DCard> AMoodList = new List<DCard>();
        foreach (var i in inventoryCards)
        {
            if (i.cardType == DCard.eCardType.mood)
            {
                var card = i.GetCard() as MoodCard;
                if (card.GetMoodValue(Note.Mood.A) > 0)
                {
                    AMoodList.Add(i);
                }
            }
        }

        List<DCard> BMoodList = new List<DCard>();
        foreach (var i in inventoryCards)
        {
            if (i.cardType == DCard.eCardType.mood)
            {
                var card = i.GetCard() as MoodCard;
                if (card.GetMoodValue(Note.Mood.B) > 0)
                {
                    BMoodList.Add(i);
                }
            }
        }

        List<DCard> CMoodList = new List<DCard>();
        foreach (var i in inventoryCards)
        {
            if (i.cardType == DCard.eCardType.mood)
            {
                var card = i.GetCard() as MoodCard;
                if (card.GetMoodValue(Note.Mood.C) > 0)
                {
                    CMoodList.Add(i);
                }
            }
        }

        List<DCard> comboList = new List<DCard>();
        foreach (var i in inventoryCards)
        {
            if (i.cardType == DCard.eCardType.combo)
            {
                comboList.Add(i);
            }
        }

        List<DCard> actionList = new List<DCard>();
        foreach (var i in inventoryCards)
        {
            if (i.cardType == DCard.eCardType.combo)
            {
                actionList.Add(i);
            }
        }


        List<DCard> chosenInventoryList = inventoryCards;
        if (currentCategory == eCardType.moodA)
        {
           chosenInventoryList = AMoodList;
        }
        else if (currentCategory == eCardType.moodB)
        {
            chosenInventoryList = BMoodList;
        }
        else if (currentCategory == eCardType.moodC)
        {
            chosenInventoryList = CMoodList;
        }
        else if (currentCategory == eCardType.Combo)
        {
            chosenInventoryList = comboList;
        }
        else if (currentCategory == eCardType.Action)
        {
            chosenInventoryList = actionList;
        }
        else if (currentCategory == eCardType.All)
        {
            chosenInventoryList = inventoryCards;
        }


        DMUtils.BuildList<UICard, DCard>(OnBuildCardInventory, chosenInventoryList.ToArray(), cardInventory.gameObject, cardInventory.transform.parent);


        //DMUtils.BuildList<UICard, DCard>(OnBuildCardInventory, inventoryCards.ToArray(), cardInventory.gameObject, cardInventory.transform.parent);
        DMUtils.BuildList<UICard, DCard>(OnBuildCardDeck, selectedCards.ToArray(), cardDeck.gameObject, cardDeck.transform.parent);
       // DMUtils.BuildList<UICard, DCard>(OnBuildCardDeck, comboList.ToArray(), cardDeck.gameObject, cardDeck.transform.parent);
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
