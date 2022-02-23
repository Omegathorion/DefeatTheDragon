using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EmeraldMirror : Relic, ITargetPlayer
{
    [Header("Player Targeting")]
    public GameObject playerTargeterPrefab;
    GameObject instantiatedTargeter;

    [Header("EmeraldReflect")]
    public int cardsPlayedThisTurn;
    public int amountToBlock;
    public int thresholdToBlock;

    //cards that need to be played is 4


    public void OnCardPlayed()
    {
        cardsPlayedThisTurn++;

        if (cardsPlayedThisTurn == thresholdToBlock)
        {
            AddBlockPlayer();
        }
    }

    public void AddBlockPlayer()
    {
        instantiatedTargeter = Instantiate(playerTargeterPrefab);
        instantiatedTargeter.GetComponent<Targeter>().requester = gameObject;
        //add block
    }

    public void ReceivePlayerTarget(GameObject player)
    {
        player.GetComponent<Player>().TakeBlock(gameObject, amountToBlock);
        ResetCardPlayedCount();
        Destroy(instantiatedTargeter);
    }

    public void RemoveStatus(GameObject status)
    {
        if (cardsPlayedThisTurn == thresholdToBlock)
        {
           //not sure how to remove status
        }
        
    }

    public void ResetCardPlayedCount()
    {
        cardsPlayedThisTurn = 0;
    }
}