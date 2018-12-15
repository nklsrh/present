using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCard
{
    public string id;
    public int index;
    public string name;
    public string description;
    public enum eCardType
    {
        mood,
        combo,
        draw,
    }
    public eCardType cardType;
    public int cardsToDraw;
    public string comboMood;
    public float comboMultiplier;
    public string[] moodChanges;

    public Card GetCard()
    {
        Card c = new Card()
        {
            name = this.name,
        };

        if (cardType == eCardType.mood)
        {
            c = new MoodCard()
            {
                name = this.name,
                moodChanges = new List<MoodValue>()
            };
            var cm = c as MoodCard;
            for (int i = 0; i < moodChanges.Length / 2; i++)
            {
                int amount = int.Parse(moodChanges[i * 2 + 1]);
                if (amount > 0)
                {
                    cm.moodChanges.Add(new MoodValue()
                    {
                        mood = (Note.Mood)System.Enum.Parse(typeof(Note.Mood), moodChanges[i * 2], true),
                        value = amount,
                    });
                }
            }

            return cm;
        }

        if (cardType == eCardType.combo)
        {
            var cm = new ComboCard()
            {
                name = this.name,
            };
            cm.multiplier = comboMultiplier;
            cm.mood = (Note.Mood)System.Enum.Parse(typeof(Note.Mood), comboMood, true);
            return cm;
        }

        if (cardType == eCardType.draw)
        {
            var cm = new DrawCard()
            {
                name = this.name,
            };
            cm.cardsToDraw = cardsToDraw;
            return cm;
        }

        return c;
    }
}
