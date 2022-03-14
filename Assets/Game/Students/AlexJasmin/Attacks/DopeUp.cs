using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DopeUp : EnemyAction
{
    public int strengthAmount = 3;
    public int blockAmount = 10;
    public GameObject playerTargeterPrefab;
    public GameObject strengthPrefab;
    public GameObject blockPrefab;
    GameObject instantiatedTargeter;
    GameObject playerTarget;

    public override void Execute()
    {
        base.Execute();
        transform.root.DOShakePosition(animationTime, 1, 10, 90, false, true);

        instantiatedTargeter = Instantiate(playerTargeterPrefab);
        instantiatedTargeter.GetComponent<Targeter>().requester = this.gameObject;

        GameObject instantiatedStrength = Instantiate(strengthPrefab);
        instantiatedStrength.GetComponent<Status>().value = strengthAmount;
        transform.root.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedStrength);

        GameObject instantiatedBlock = Instantiate(blockPrefab);
        instantiatedBlock.GetComponent<Status>().value = blockAmount;
        transform.root.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedBlock);

        Destroy(instantiatedTargeter);
    }
}
