using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistIncantation : EnemyAction
{
    public int ritualAmount;
    public float specialRitualScaling;
    public GameObject ritualPrefab;

    public override void Execute()
    {
        base.Execute();
        GameObject instantiatedRitual = Instantiate(ritualPrefab);

        GameObject instantiatedProcessor = Instantiate(processorPrefab);
        instantiatedProcessor.GetComponent<InterjectionProcessor>().startingValue = Mathf.FloorToInt(ritualAmount + (ritualAmount * (transform.root.GetComponent<Enemy>().difficultyModifier - 1) * specialRitualScaling));
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, transform.root.gameObject, InteractionType.Status, instantiatedProcessor.GetComponent<InterjectionProcessor>()));

        instantiatedRitual.GetComponent<Status>().value = instantiatedProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();

        transform.root.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedRitual);

        Destroy(instantiatedProcessor);
    }
}
