using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RagerAttack : EnemyAction, ITargetPlayer
{
    public int baseDamage = 2;
	public int damageIncreasePerTurn = 2;
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

        GameObject baseDamageProcessor = Instantiate(processorPrefab);
        baseDamageProcessor.GetComponent<InterjectionProcessor>().startingValue = Mathf.FloorToInt(baseDamage);
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, playerTarget, InteractionType.Damage, baseDamageProcessor.GetComponent<InterjectionProcessor>()));
        playerTarget.GetComponent<ITakeDamage>().TakeDamage(this.gameObject, baseDamageProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue());
		this.baseDamage += this.damageIncreasePerTurn;
        Destroy(baseDamageProcessor);
        Destroy(instantiatedTargeter);
    }
}
