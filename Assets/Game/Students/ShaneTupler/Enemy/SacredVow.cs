using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacredVow : EnemyAction
{
    public int healAmount;

    public override void Execute()
    {
        base.Execute();
        GameObject instantiatedProcessor = Instantiate(processorPrefab);
        instantiatedProcessor.GetComponent<InterjectionProcessor>().startingValue = Mathf.FloorToInt(healAmount + (healAmount * (transform.root.GetComponent<Enemy>().difficultyModifier - 1)));
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, transform.root.gameObject, InteractionType.Status, instantiatedProcessor.GetComponent<InterjectionProcessor>()));

        transform.root.GetComponent<ITakeDamage>().TakeHealing(this.gameObject, healAmount);

        Destroy(instantiatedProcessor);
    }
}
