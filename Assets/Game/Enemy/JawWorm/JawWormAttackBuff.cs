using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class JawWormAttackBuff : EnemyAction
{
    public GameObject statusPrefab;
    public int startingStatusAmount;

    public override void Execute()
    {
        base.Execute();

        GameObject instantiatedStatus = Instantiate(statusPrefab);
        GameObject instantiatedProcessor = Instantiate(processorPrefab);
        instantiatedProcessor.GetComponent<InterjectionProcessor>().startingValue = startingStatusAmount;
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, transform.root.gameObject, InteractionType.Status, instantiatedProcessor.GetComponent<InterjectionProcessor>()));
        instantiatedStatus.GetComponent<Status>().value = instantiatedProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
        transform.root.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedStatus);
        Destroy(instantiatedProcessor);
    }
}
