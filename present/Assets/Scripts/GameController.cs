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

    public const float TAP_NODE_TIME_WINDOW = 0.5f;       // how much time before and after the exact note point do we allow the user to tap
    public const int PENALTY = 2;

    bool gameStarted = false;

    public Menu_Endgame menu_Endgame;
    public Menu_Game menu_Game;

    public StageRandomiser stageRandomiser;

    public Note comboStartNote;
    public int comboScore;
    public Note.Mood comboMood;

    public void Setup(DLevel level, List<Card> deckCards)
    {
        this.level = level;

        stageRandomiser.Randomise();

        SetupSong(level.id);
        SetupDeck(deckCards);

        SetupHand();

        SetupScore(level.startA, level.startB, level.startC, level.scoreA, level.scoreB, level.scoreC);

        currentTime = 0;

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
                //OnLevelFinished();
                menu_Game.AnimateScore(moodValues[0].mood, moodValues[0].value);
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

        for (int i = 0; i < card.moodChanges.Count; i++)
        {
            var moodChange = card.moodChanges[i];

            var mood = moodChange.mood;
            int score = moodChange.value;

            if (note.moodScores.ContainsKey(mood))
            {
                var values = moodValues.FindAll(r => r.mood == mood);
                for (int j = 0; j < values.Count; j++)
                {
                    int delta = score - note.moodScores[mood];
                    values[j].value += delta;
                    uiScoring.SetScore(values[j].mood, values[j].value);

                    menu_Game.AnimateScore(values[j].mood, delta);
                }
                comboStartNote = note;
            }
            else
            {
                // card missing this mood, convert into Anger
                numberOfMissedSlots++;
            }
        }

        if (numberOfMissedSlots == 0)
        {
            // card used up all the mood slots, excellent, now reduce Anger

        }
    }

    List<MoodValue> GetMoodValues(Note note, Note.Mood mood)
    {
        return moodValues.FindAll(r => r.mood == Note.Mood.Blank || note.moodScores.ContainsKey(r.mood));
    }

    public void OnLevelFinished()
    {
        menu_Endgame.Show();
        menu_Game.gameObject.SetActive(false);

        gameStarted = false;
    }
}
