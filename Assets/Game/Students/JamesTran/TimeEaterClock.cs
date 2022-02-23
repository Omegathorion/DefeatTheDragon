using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class TimeEaterClock : Relic
{
    [Header("Cards Played")]
    public int cardsPlayedThisTurn;
    public int maxCardsPlayed;

    public GameEvent playerTurnEnd;

    public void OnCardPlayed()
    {
        cardsPlayedThisTurn++;
        if(cardsPlayedThisTurn >= maxCardsPlayed)
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
        playerTurnEnd.Raise();
    }


}