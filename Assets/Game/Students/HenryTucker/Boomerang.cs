using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;


public class Boomerang : MonoBehaviour
{
    [SerializeField] DeckManager deckManager;
    public int amountOfCardsDrawnFromDiscard;
    public GameObjectGameEvent drawCardEvent;

    public void WhenCardsAreDrawnAtTurnStart()
    {
        deckManager = transform.root.GetComponentInChildren<DeckManager>();
        Card[] discardedCards = deckManager.discard.GetComponentsInChildren<Card>();
        if (discardedCards.Length >= 1)
        {
            GameObject cardDrawn = discardedCards[Random.Range(0, discardedCards.Length)].gameObject;
            deckManager.CardWasDrawn(cardDrawn);
            drawCardEvent.Raise(cardDrawn);
        }
        //GameObject cardFromDiscard = discardPile.GetChild(Random.Range(0, discardPile.GetChild.length).gameObject);
    }
}
