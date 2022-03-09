using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceStrike : EnemyAction, ITargetPlayer
{

    public Sentinel sentinel;
    public int damage;
    public GameObject playerTargeterPrefab;
    GameObject instantiatedTargeter;
    GameObject playerTarget;

    public override void Execute()
    {
        base.Execute();

        instantiatedTargeter = Instantiate(playerTargeterPrefab);
        instantiatedTargeter.GetComponent<Targeter>().requester = this.gameObject;
    }

    public void ReceivePlayerTarget(GameObject player)
    {
        playerTarget = player;

        GameObject damageProcessor = Instantiate(processorPrefab);
        damageProcessor.GetComponent<InterjectionProcessor>().startingValue = Mathf.FloorToInt(damage + (sentinel.cardsPlayedThisTurn * sentinel.damagePerCard) * transform.root.GetComponent<Enemy>().difficultyModifier);
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, playerTarget, InteractionType.Damage, damageProcessor.GetComponent<InterjectionProcessor>()));
        playerTarget.GetComponent<ITakeDamage>().TakeDamage(this.gameObject, damageProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue());
        Destroy(damageProcessor);
        Destroy(instantiatedTargeter);
    }
}