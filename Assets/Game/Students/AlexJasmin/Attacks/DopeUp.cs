using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DopeUp : EnemyAction
{
    public int strengthAmount = 3;
    public GameObject playerTargeterPrefab;
    public GameObject strengthPrefab;
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

        GameObject instantiatedStrength = Instantiate(strengthPrefab);
        instantiatedStrength.GetComponent<Status>().value = strengthAmount;
        transform.root.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedStrength);

        Destroy(instantiatedTargeter);
    }
}
