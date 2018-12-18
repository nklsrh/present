using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public const float TAP_NODE_TIME_WINDOW = 0.6f;       // how much time before and after the exact note point do we allow the user to tap
    public const int PENALTY = 2;

    bool gameStarted = false;

    public Menu_Endgame menu_Endgame;
    public Menu_Game menu_Game;

    public StageRandomiser stageRandomiser;

    public Note comboStartNote;
    public int comboScore;
    public Note.Mood comboMood;

    public int currentAngerAmount;

    public void Setup(DLevel level, List<Card> deckCards)
    {
        this.level = level;

        stageRandomiser.Randomise();

        SetupSong(level.id);
        SetupDeck(deckCards);

        SetupHand();

        currentAngerAmount = level.startAnger;

        SetupScore(level.startA, level.startB, level.startC, level.scoreA, level.scoreB, level.scoreC, level.startAnger, level.angerLimit);

        currentTime = 0;

        menu_Game.Reset();

        uiTimeline.Setup(song);
        uiTimeline.onMissedNote += OnMissedNote;

        gameStarted = true;
    }

    private void SetupSong(string id)
    {
        song = new Song();
        song.notes = new List<Note>();

        var timings = Data.timings[id];
        float lastTime = 0.0f;

        for (int i = 0; i < timings.moodChanges.Length; i++)
        {
            var note = new Note();

            var splitted = level.moodChanges[i].Split(';');
            note.moodScores = new Dictionary<Note.Mood, int>();
            for (int j = 0; j < splitted.Length; j++)
            {
                int val = int.Parse(splitted[j]);
                if (val > 0)
                {
                    note.moodScores.Add((Note.Mood)j, val);
                }
            }

            note.time = lastTime + float.Parse(timings.moodChanges[i].Split('_')[0]);
            lastTime = note.time;

            note.isComboNote = timings.moodChanges[i].Contains("COMBO");

            song.notes.Add(note);
        }
    }

    private void SetupScore(int startA, int startB, int startC, int a, int b, int c, int startAnger, int angerLimit)
    {
        moodValues = new List<MoodValue>();
        moodValues.Add(new MoodValue() { mood = Note.Mood.A, value = startA });
        moodValues.Add(new MoodValue() { mood = Note.Mood.B, value = startB });
        moodValues.Add(new MoodValue() { mood = Note.Mood.C, value = startC });
        moodValues.Add(new MoodValue() { mood = Note.Mood.Blank, value = 0 });

        uiScoring.SetScore(Note.Mood.A, startA, a);
        uiScoring.SetScore(Note.Mood.B, startB, b);
        uiScoring.SetScore(Note.Mood.C, startC, c);

        uiScoring.SetAngerMax(angerLimit);
        uiScoring.SetAnger(startAnger);
    }

    private void SetupDeck(List<Card> deckCards)
    {
        deck = deckCards;
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
                LevelComplete();
            }

            if (uiTimeline.IsLevelComplete())
            {
                LevelComplete();
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
                DrawCards(1);
            }
            BreakCombo();
        }

        if (card is ComboCard)
        {
            var c = card as ComboCard;
            if (note != null)
            {
                if (note.isComboNote)
                {
                    // check if starting notes contains any mood from the current note
                    comboMood = comboStartNote.GetFirstMatchingMood(note);

                    if (comboMood != Note.Mood.Blank)
                    {
                        PlayCombo(note, c);
                    }
                }
                DrawCards(1);
            }
        }

        if (card is DrawCard)
        {
            var c = card as DrawCard;

            if (note != null)
            {
                DrawCards(c.cardsToDraw);
            }
            BreakCombo();
        }

        uiHand.Setup(hand);
    }

    private void BreakCombo()
    {
        comboScore = 0;
    }

    private void PlayCombo(Note note, ComboCard c)
    {
        comboScore += note.moodScores[c.mood] * c.multiplier;

        var v = GetMoodValues(note, c.mood);

        for (int i = 0; i < v.Count; i++)
        {
            v[i].value += comboScore;

            Debug.Log("ADd combo score to value for " + v[i].mood + " taking it from " + (v[i].value - comboScore) + " to " + v[i].value);
        }

        //5 + (2) = 2
        // 200% = 4
        // 300% = 12
    }

    private void OnMissedNote(UITapPoint tap)
    {
        for (int j = 0; j < moodValues.Count; j++)
        {
            //Debug.Log("PENALTY: " + moodValues[j].mood + " : " + moodValues[j].value);
            moodValues[j].value -= PENALTY;

            uiScoring.SetScore(moodValues[j].mood, moodValues[j].value);
        }

        if (hand.Count == 0)
        {
            DrawCards(1);
        }
    }

    private void ScoreMood(Note note, MoodCard card)
    {
        int numberOfMissedSlots = 0;
        int angerAmount = 0;

        foreach (var noteMood in note.moodScores)
        {
            bool hasMatchedWithCard = false;
            foreach (var cardSlot in card.moodChanges)
            {
                // if player's card has the required mood
                if (cardSlot.mood == noteMood.Key)
                {
                    var gameScore = moodValues.Find(r => r.mood == cardSlot.mood);

                    if (gameScore != null)
                    {
                        int delta = cardSlot.value - noteMood.Value;

                        int actualChange = 0;

                        // max we add is the value from the note (don't go over that)
                        if (delta > 0)
                        {
                            actualChange = note.moodScores[cardSlot.mood];
                        }
                        else // otherwise take the value from the card only
                        {
                            actualChange = cardSlot.value;
                        }

                        gameScore.value += actualChange;

                        if (delta < 0) // if less than required, add to Anger
                        {
                            angerAmount += -delta;
                        }

                        uiScoring.SetScore(gameScore.mood, gameScore.value);

                        if (actualChange > 0)
                        {
                            menu_Game.AnimateScore(gameScore.mood, actualChange);
                        }

                        hasMatchedWithCard = true;
                    }
                    comboStartNote = note;
                }
                else
                {
                    // if card has a mood not in the note, don't do anything with it
                }
            }

            if (!hasMatchedWithCard)
            {
                // if you played a card that doesn't have required mood from the Note, convert the note to Anger

                numberOfMissedSlots++;
                angerAmount += noteMood.Value;
            }
        }

        if (angerAmount > 0)
        {
            int newAngerAmount = currentAngerAmount + angerAmount;
            uiScoring.SetAnger(newAngerAmount);

            menu_Game.AnimateAnger(angerAmount);

            currentAngerAmount = newAngerAmount;
        }
        else if (numberOfMissedSlots == 0)
        {
            // card used up all the mood slots, excellent, now reduce Anger

            currentAngerAmount -= level.angerReduction;

            if (currentAngerAmount < 0)
            {
                currentAngerAmount = 0;
            }
        }

        if (currentAngerAmount >= level.angerLimit)
        {
            EndLevelBecauseAnger();
        }
    }

    List<MoodValue> GetMoodValues(Note note, Note.Mood mood)
    {
        return moodValues.FindAll(r => r.mood == Note.Mood.Blank || note.moodScores.ContainsKey(r.mood));
    }

    private void EndLevelBecauseAnger()
    {
        menu_Game.AnimateAngerFail();
        OnLevelFinished();
    }

    private void LevelComplete()
    {
        menu_Game.AnimateLevelFinish();
        OnLevelFinished();
    }

    public void OnLevelFinished()
    {
        StartCoroutine(WaitThenFinishLevel());

        gameStarted = false;
    }

    IEnumerator WaitThenFinishLevel()
    {
        yield return new WaitForSeconds(5.0f);

        menu_Endgame.Show();
        menu_Game.gameObject.SetActive(false);
    }
}
