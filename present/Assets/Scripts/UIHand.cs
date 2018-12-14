using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHand : MonoBehaviour {

    [Header("Fill these up")]
    public UICard card;

    [Header("Ignore these")]
    public List<UICard> cards;

    internal void Setup(List<Card> deck)
    {
        cards.Clear();

        var cardArray = deck.ToArray();
        DMUtils.BuildList<UICard, Card>(OnBuildCard, cardArray, card.gameObject, card.transform.parent);
    }

    private void OnBuildCard(UICard ui, Card card)
    {
        ui.SetCard(card);
    }
}
