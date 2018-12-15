using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHand : MonoBehaviour {

    [Header("Fill these up")]
    public UICard card;

    [Header("Ignore these")]
    public List<UICard> cards;

    private Action<Card> onPlayCard;

    internal void Setup(List<Card> cardList, Action<Card> playCard)
    {
        this.onPlayCard = playCard;

        Setup(cardList);
    }

    internal void Setup(List<Card> cardList)
    {
        cards.Clear();

        var cardArray = cardList.ToArray();
        DMUtils.BuildList<UICard, Card>(OnBuildCard, cardArray, card.gameObject, card.transform.parent);
    }

    private void OnBuildCard(UICard ui, Card card)
    {
        ui.SetCard(card);

        if (onPlayCard != null)
        {
            ui.SetAction(onPlayCard);
        }
    }
}
