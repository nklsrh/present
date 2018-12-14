using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UITimeline uiTimeline;
    public UIHand uiHand;

    public Song song;

    public List<Card> hand;
    public List<Card> deck;

    public float currentTime;

    public List<MoodValue> moodValues; 

    void Start ()
    {
        SetupMoods();

        SetupDeck();

        SetupHand();

        uiTimeline.Setup(song);
    }

    private void SetupDeck()
    {
        deck = new List<Card>();

        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 1 } } });
        deck.Add(new DrawCard() { cardsToDraw = 3 });
        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 1 } } });
        deck.Add(new DrawCard() { cardsToDraw = 3 });
        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 1 } } });
        deck.Add(new DrawCard() { cardsToDraw = 3 });
        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 1 } } });
        deck.Add(new DrawCard() { cardsToDraw = 3 });
        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 1 } } });
        deck.Add(new DrawCard() { cardsToDraw = 3 });
        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 1 } } });
        deck.Add(new DrawCard() { cardsToDraw = 3 });
    }

    public void SetupHand()
    {
        hand = new List<Card>();

        DrawCards(5);

        uiHand.Setup(hand);
    }

    private void DrawCards(int number)
    {
        for (int i = 0; i < number && deck.Count > 0; i++)
        {
            int chosen = UnityEngine.Random.Range(0, deck.Count);
            var card = deck[chosen];
            deck.Remove(card);
            hand.Add(card);
        }

        uiHand.Setup(hand);
    }

    private void SetupMoods()
    {
        moodValues = new List<MoodValue>();
        moodValues.Add(new MoodValue() { mood = Note.Mood.A, value = 0 });
        moodValues.Add(new MoodValue() { mood = Note.Mood.B, value = 0 });
        moodValues.Add(new MoodValue() { mood = Note.Mood.C, value = 0 });
        moodValues.Add(new MoodValue() { mood = Note.Mood.Blank, value = 0 });
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        uiTimeline.UpdateLogic(currentTime);

        if (Input.GetKeyDown(KeyCode.D))
        {
            DrawCards(1);
        }
    }
}
