using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public UITimeline uiTimeline;
    public UIHand uiHand;
    public UIScoring uiScoring;

    public Song song;

    public List<Card> hand;
    public List<Card> deck;

    public float currentTime;

    public List<MoodValue> moodValues;

    public List<MoodValue> moodRequired;

    public const int MAX_HAND_SIZE = 7;
    public const int STARTING_HAND_SIZE = 4;

    public const float TAP_NODE_TIME_WINDOW = 0.5f;       // how much time before and after the exact note point do we allow the user to tap

    void Start ()
    {
        SetupMoods();

        SetupDeck();

        SetupHand();

        SetupScore();

        uiTimeline.Setup(song);
    }

    private void SetupScore()
    {
        uiScoring.SetScore(Note.Mood.A, 0, 100);
        uiScoring.SetScore(Note.Mood.B, 0, 100);
        uiScoring.SetScore(Note.Mood.C, 0, 100);
    }

    private void SetupDeck()
    {
        deck = new List<Card>();

        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 3 }, new MoodValue() { mood = Note.Mood.B, value = 3 } } });
        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 3 } } });
        deck.Add(new DrawCard() { cardsToDraw = 3 });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 1 } } });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 2 } } });
        deck.Add(new DrawCard() { cardsToDraw = 2 });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 5 }, new MoodValue() { mood = Note.Mood.C, value = 4 } } });
        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 1 } } });
        deck.Add(new DrawCard() { cardsToDraw = 3 });
        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 2 }, new MoodValue() { mood = Note.Mood.B, value = 1 } } });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 4 } } });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 3 }, new MoodValue() { mood = Note.Mood.C, value = 2 } } });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 4 } } });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 3 } } });
        deck.Add(new ComboCard() { mood = Note.Mood.A, multiplier = 2.0f });
        deck.Add(new MoodCard() { moodChanges = new List<MoodValue>() { new MoodValue() { mood = Note.Mood.A, value = 2 }, new MoodValue() { mood = Note.Mood.B, value = 5 } } });
        deck.Add(new DrawCard() { cardsToDraw = 2 });
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

    public Note GetNote(float time)
    {
        return song.GetNote(time);
    }

    public void PlayCard(Card card)
    {
        hand.Remove(card);

        if (card is MoodCard)
        {
            var c = card as MoodCard;

            var note = GetNote(currentTime);
            if (note != null)
            {
                var moodMatches = c.moodChanges.FindAll(r => note.moods.Contains(r.mood));
                if (note != null)
                {
                    //if (moodMatches.Count > 0)
                    {
                        ScoreMood(note, c, moodMatches);
                    }
                }
            }
        }

        if (card is ComboCard)
        {
            var c = card as ComboCard;

            var note = GetNote(currentTime);
            if (note != null)
            {
                Debug.Log("COMBO?");
            }
        }

        if (card is DrawCard)
        {
            var c = card as DrawCard;

            DrawCards(c.cardsToDraw);
        }
        else
        {
            DrawCards(1);
        }

        uiHand.Setup(hand);
    }

    private void ScoreMood(Note note, MoodCard card, List<MoodValue> matches)
    {
        for (int i = 0; i < card.moodChanges.Count; i++)
        {
            var moodChange = card.moodChanges[i];

            var mood = moodChange.mood;
            int score = moodChange.value;

            var values = moodValues.FindAll(r => (note.moods.Count == 0 || r.mood == Note.Mood.Blank) && mood == r.mood);
            for (int j = 0; j < values.Count; j++)
            {
                values[j].value += score - note.score;
                uiScoring.SetScore(values[j].mood, values[j].value, 100);

                Debug.Log(values[j].mood + " CCCSCORE! " + values[j].value);
            }




            Debug.Log(card.moodChanges[i].mood + " SCORE! " + card.moodChanges[i].value);
        }
    }
}
