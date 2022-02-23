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
    public GameObject blockPrefab;
    public GameObject processorPrefab;
    public CallForInterjectionsGameEvent interjectionEvent;
    public GameObjectCollection naughtyStatuses;

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
    }

    public void ReceivePlayerTarget(GameObject player)
    {
        GameObject blockProcessor = Instantiate(processorPrefab);
        blockProcessor.GetComponent<InterjectionProcessor>().startingValue = amountToBlock;
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, player, InteractionType.Block, blockProcessor.GetComponent<InterjectionProcessor>()));

        GameObject instantiatedBlock = Instantiate(blockPrefab);
        instantiatedBlock.GetComponent<Status>().value = blockProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();

        player.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedBlock);


        ResetCardPlayedCount();
        RemoveNegativeStatuses(player);
        Destroy(instantiatedTargeter);
    }

    public void RemoveNegativeStatuses(GameObject player)
    {
        Status[] statuses = player.GetComponentsInChildren<Status>();
        foreach (Status eachStatus in statuses)
        {
            foreach (GameObject eachNaughtyStatus in naughtyStatuses)
            {
                if (eachStatus.GetType() == eachNaughtyStatus.GetComponent<Status>().GetType())
                {
                    Destroy(eachStatus.gameObject);
                    break;
                }
            }
        }
    }

    public void ResetCardPlayedCount()
    {
        cardsPlayedThisTurn = 0;
    }
}