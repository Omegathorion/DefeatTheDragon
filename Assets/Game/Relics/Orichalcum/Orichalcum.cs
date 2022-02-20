using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Orichalcum : Relic, ITargetPlayer
{
    public GameObject playerTargeterPrefab;
    GameObject instantiatedTargeter;
    public GameObject processorPrefab;
    GameObject instantiatedProcessor;
    public CallForInterjectionsGameEvent interjectionEvent;
    GameObject playerTarget;
    public GameObject blockPrefab;

    public int blockAmount;

    public void OnPlayerTurnEnd()
    {
        if (!transform.root.GetComponentInChildren<Block>())
        {
            GiveThePlayerSomeBlock();
        }
        else
        {
            Block playerBlock = transform.root.GetComponentInChildren<Block>();
            if (playerBlock.value == 0)
            {
                GiveThePlayerSomeBlock();
            }
        }
    }

    public void GiveThePlayerSomeBlock()
    {
        instantiatedTargeter = Instantiate(playerTargeterPrefab);
        instantiatedTargeter.GetComponent<Targeter>().requester = this.gameObject;
    }

    public void ReceivePlayerTarget(GameObject player)
    {
        playerTarget = player;

        GameObject blockProcessor = Instantiate(processorPrefab);
        blockProcessor.GetComponent<InterjectionProcessor>().startingValue = blockAmount;
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, playerTarget, InteractionType.Block, blockProcessor.GetComponent<InterjectionProcessor>()));

        GameObject instantiatedBlock = Instantiate(blockPrefab);
        instantiatedBlock.GetComponent<Status>().value = blockProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();

        playerTarget.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedBlock);
        Destroy(blockProcessor);
        Destroy(instantiatedTargeter);
    }
}
