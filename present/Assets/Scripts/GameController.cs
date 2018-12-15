using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UITimeline uiTimeline;
    public UIHand uiHand;
    public UIScoring uiScoring;

    public DLevel level;
    public Song song;

    public List<Card> hand;
    public List<Card> deck;

    public float currentTime;

    public List<MoodValue> moodValues;

    public List<MoodValue> moodRequired;

    public const int MAX_HAND_SIZE = 7;
    public const int STARTING_HAND_SIZE = 4;

    public const float TAP_NODE_TIME_WINDOW = 0.5f;       // how much time before and after the exact note point do we allow the user to tap
    public const int PENALTY = 2;

    bool gameStarted = false;

    public Menu_Endgame menu_Endgame;
    public GameObject menu_Game;

    public void Setup(DLevel level, List<Card> deckCards)
    {
        this.level = level;

        SetupDeck(deckCards);

        SetupHand();

        SetupScore(level.startA, level.startB, level.startC, level.scoreA, level.scoreB, level.scoreC);

        currentTime = 0;

        uiTimeline.Setup(song);
        uiTimeline.onMissedNote += OnMissedNote;

        gameStarted = true;
    }

    private void SetupScore(int startA, int startB, int startC, int a, int b, int c)
    {
        moodValues = new List<MoodValue>();
        moodValues.Add(new MoodValue() { mood = Note.Mood.A, value = startA });
        moodValues.Add(new MoodValue() { mood = Note.Mood.B, value = startB });
        moodValues.Add(new MoodValue() { mood = Note.Mood.C, value = startC });
        moodValues.Add(new MoodValue() { mood = Note.Mood.Blank, value = 0 });

        uiScoring.SetScore(Note.Mood.A, startA, a);
        uiScoring.SetScore(Note.Mood.B, startB, b);
        uiScoring.SetScore(Note.Mood.C, startC, c);
    }

    private void SetupDeck(List<Card> deckCards)
    {
        deck = deckCards;

        //deck = new List<Card>();

        //deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 3 }, new MoodValue() { mood = Note.Mood.B, value = 3 } } });
        //deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 3 } } });
        //deck.Add(new DrawCard() { cardsToDraw = 3 });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 7 } } });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 2 } } });
        //deck.Add(new DrawCard() { cardsToDraw = 2 });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 5 }, new MoodValue() { mood = Note.Mood.C, value = 4 } } });
        //deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 5 } } });
        //deck.Add(new DrawCard() { cardsToDraw = 3 });
        //deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 2 }, new MoodValue() { mood = Note.Mood.B, value = 4 } } });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 4 } } });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 3 }, new MoodValue() { mood = Note.Mood.C, value = 2 } } });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 4 } } });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 3 } } });
        //deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        //deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 2 }, new MoodValue() { mood = Note.Mood.B, value = 5 } } });
        //deck.Add(new DrawCard() { cardsToDraw = 2 });
    }

    public void SetupHand()
    {
        hand = new List<Card>();

        DrawCards(STARTING_HAND_SIZE);

        uiHand.Setup(hand, PlayCard);
    }

    private void DrawCards(int number)
    {
        for (int i = 0; i < number && deck.Count > 0 && hand.Count < MAX_HAND_SIZE; i++)
        {
            int chosen = UnityEngine.Random.Range(0, deck.Count);
            var card = deck[chosen];
            deck.Remove(card);
            hand.Add(card);
        }

        uiHand.Setup(hand);
    }

    private void Update()
    {
        if (gameStarted)
        {
            currentTime += Time.deltaTime;

            uiTimeline.UpdateLogic(currentTime);

            if (Input.GetKeyDown(KeyCode.D))
            {
                //DrawCards(1);
                OnLevelFinished();
            }

            if (uiTimeline.IsLevelComplete())
            {
                OnLevelFinished();
            }
        }
    }

    public Note GetNote(float time)
    {
        return song.GetNote(time);
    }

    public void PlayCard(Card card)
    {
        hand.Remove(card);

        var note = GetNote(currentTime);
        if (note != null)
        {
            uiTimeline.HideLatestNode();
        }

        if (card is MoodCard)
        {
            var c = card as MoodCard;
            if (note != null)
            {
                ScoreMood(note, c);
            }
            DrawCards(1);
        }

        if (card is ComboCard)
        {
            var c = card as ComboCard;
            if (note != null)
            {
                if (note.isComboNote)
                {
                    //var value = c.multiplier;
                }
            }
            DrawCards(1);
        }

        if (card is DrawCard)
        {
            var c = card as DrawCard;

            if (note != null)
            {
                DrawCards(c.cardsToDraw);
            }
            else
            {
                DrawCards(1);
            }
        }

        uiHand.Setup(hand);
    }

    private void OnMissedNote(UITapPoint tap)
    {
        for (int j = 0; j < moodValues.Count; j++)
        {
            //Debug.Log("PENALTY: " + moodValues[j].mood + " : " + moodValues[j].value);
            moodValues[j].value -= PENALTY;

            uiScoring.SetScore(moodValues[j].mood, moodValues[j].value);
        }
    }

    private void ScoreMood(Note note, MoodCard card)
    {
        for (int i = 0; i < card.moodChanges.Count; i++)
        {
            var moodChange = card.moodChanges[i];

            var mood = moodChange.mood;
            int score = moodChange.value;

            var values = moodValues.FindAll(r => r.mood == mood);

            for (int j = 0; j < values.Count; j++)
            {
                //Debug.Log("Matches: " + values[j].mood + " : " + values[j].value);
                values[j].value += score - note.score;
                uiScoring.SetScore(values[j].mood, values[j].value);
            }
        }
    }

    List<MoodValue> GetMoodValues(Note note, Note.Mood mood)
    {
        return moodValues.FindAll(r => r.mood == Note.Mood.Blank || note.moods.Contains(r.mood));
    }

    public void OnLevelFinished()
    {
        menu_Endgame.Show();
        menu_Game.SetActive(false);

        gameStarted = false;
    }
}
