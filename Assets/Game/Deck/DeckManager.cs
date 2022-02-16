using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System;
using System.Linq;
using System.Security.Cryptography;

public class DeckManager : MonoBehaviour
{
    public Transform hand;
    public Transform deck;
    public Transform discard;
    public Transform exhaust;

    public GameObjectGameEvent drawCardEvent;
    public GameObjectGameEvent putCardBackIntoDrawPileEvent;
    public GameObjectGameEvent discardCardEvent;

    void Start()
    {
        //ShuffleDeck();
    }

    void Update()
    {
        
    }

    public void DrawCardsFromTopOfDeck(int amountOfCardsToDraw)
    {
        for (int cardIndex = 0; cardIndex < amountOfCardsToDraw; cardIndex++)
        {
            if (deck.childCount >= 1)
            {
                GameObject cardFromDeck = deck.GetChild(0).gameObject;
                cardFromDeck.transform.parent = hand;
                drawCardEvent.Raise(cardFromDeck);
            }
            else
            {
                ShuffleDiscardPileToDeck();
                GameObject cardFromDeck = deck.GetChild(0).gameObject;
                cardFromDeck.transform.parent = hand;
                drawCardEvent.Raise(cardFromDeck);
            }
        }
    }

    public void CardWasDiscarded(GameObject whichCardWasDiscarded)
    {
        whichCardWasDiscarded.transform.parent = discard;
        whichCardWasDiscarded.transform.localPosition = Vector3.zero;
    }

    public void ShuffleDiscardPileToDeck()
    {
        foreach (Card eachCard in discard.GetComponentsInChildren<Card>())
        {
            GameObject cardFromDiscard = eachCard.gameObject;
            cardFromDiscard.transform.parent = deck;
            putCardBackIntoDrawPileEvent.Raise(cardFromDiscard);
        }
        ShuffleDeck();
    }

    public void ShuffleAllCardsBackToDeck()
    {
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        int[] cardsInDeck = new int[deck.childCount];
        for (int index = 0; index < cardsInDeck.Length; index++)
        {
            cardsInDeck[index] = index;
        }
        System.Random random = new System.Random();
        cardsInDeck = cardsInDeck.OrderBy(x => random.Next()).ToArray();
        int currentCardIndex = 0;
        foreach (Card eachCard in deck.GetComponentsInChildren<Card>())
        {
            eachCard.transform.SetSiblingIndex(cardsInDeck[currentCardIndex]);
            currentCardIndex++;
        }
    }

    public void DiscardAllCardsInHand()
    {
        foreach (Card eachCard in hand.GetComponentsInChildren<Card>()) 
        {
            GameObject cardFromHand = eachCard.gameObject;
            cardFromHand.transform.parent = discard;
            discardCardEvent.Raise(cardFromHand);
        }
    }
}
