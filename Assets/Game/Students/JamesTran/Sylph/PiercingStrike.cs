using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingStrike : EnemyAction, ITargetPlayer
{
    public Vector2 damageRange;

    public int damage;
    public float specialDamageScaling;
    public GameObject playerTargeterPrefab;
    GameObject instantiatedTargeter;
    GameObject playerTarget;

    int numberOfAttacks;

    public void StartOfCombat()
    {
        damage = Mathf.FloorToInt(Random.Range(damageRange.x, damageRange.y + 1));
    }

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
        damageProcessor.GetComponent<InterjectionProcessor>().startingValue = Mathf.FloorToInt(damage + (damage * (transform.root.GetComponent<Enemy>().difficultyModifier - 1) * specialDamageScaling));
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, playerTarget, InteractionType.Damage, damageProcessor.GetComponent<InterjectionProcessor>()));
        playerTarget.GetComponent<ITakeDamage>().TakePiercingDamage(this.gameObject, damageProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue());
        Destroy(damageProcessor);
        Destroy(instantiatedTargeter);
    }
}
