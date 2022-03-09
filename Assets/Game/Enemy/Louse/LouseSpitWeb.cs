using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LouseSpitWeb : EnemyAction, ITargetPlayer
{
    public int weakAmount;
    public GameObject playerTargeterPrefab;
    GameObject instantiatedTargeter;
    GameObject playerTarget;
    public GameObject weakPrefab;

    public override void Execute()
    {
        base.Execute();
        transform.root.DOShakePosition(animationTime, 1, 10, 90, false, true);

        instantiatedTargeter = Instantiate(playerTargeterPrefab);
        instantiatedTargeter.GetComponent<Targeter>().requester = this.gameObject;
    }

    public void ReceivePlayerTarget(GameObject player)
    {
        playerTarget = player;

        GameObject statusProcessor = Instantiate(processorPrefab);
        statusProcessor.GetComponent<InterjectionProcessor>().startingValue = weakAmount;
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, playerTarget, InteractionType.Status, statusProcessor.GetComponent<InterjectionProcessor>()));
        GameObject instantiatedWeakness = Instantiate(weakPrefab);
        instantiatedWeakness.GetComponent<Status>().value = statusProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
        playerTarget.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedWeakness);
        Destroy(statusProcessor);
        Destroy(instantiatedTargeter);
    }
}