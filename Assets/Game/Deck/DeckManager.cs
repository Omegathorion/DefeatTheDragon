using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class DeckManager : MonoBehaviour
{
    public Transform hand;
    public Transform deck;
    public Transform discard;
    public Transform exhaust;

    public GameObjectGameEvent drawCardEvent;
    public GameObjectGameEvent putCardBackIntoDrawPileEvent;

    void Start()
    {
        
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
    }

    public void ShuffleAllCardsBackToDeck()
    {

    }
}
