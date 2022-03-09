using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Slash : EnemyAction, ITargetPlayer
{
    public int damage;
    public int weakenAmount;

    public GameObject weakenPrefab;
    public GameObject playerTargeterPrefab;
    GameObject instantiatedTargeter;
    GameObject playerTarget;

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

        GameObject damageProcessor = Instantiate(processorPrefab);
        damageProcessor.GetComponent<InterjectionProcessor>().startingValue = Mathf.FloorToInt(damage * transform.root.GetComponent<Enemy>().difficultyModifier);
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, playerTarget, InteractionType.Damage, damageProcessor.GetComponent<InterjectionProcessor>()));
        playerTarget.GetComponent<ITakeDamage>().TakeDamage(this.gameObject, damageProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue());
        Destroy(damageProcessor);
        Destroy(instantiatedTargeter);

        GameObject instantiatedweakening = Instantiate(weakenPrefab);
        GameObject weakeningProcessor = Instantiate(processorPrefab);
        weakeningProcessor.GetComponent<InterjectionProcessor>().startingValue = weakenAmount;
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, playerTarget, InteractionType.Status, weakeningProcessor.GetComponent<InterjectionProcessor>()));
        instantiatedweakening.GetComponent<Status>().value = weakeningProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
        Destroy(weakeningProcessor);
        playerTarget.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedweakening);
    }
}
