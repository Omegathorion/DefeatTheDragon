using UnityEngine;

public class CorruptCrystalization : EnemyAction
{
    [Header("Strength Settings")]
    [SerializeField] int StrengthAmount;
    [SerializeField] float StrengthScaling;
    [SerializeField] GameObject StrengthPrefab;

    public override void Execute()
    {
        base.Execute();
        GameObject instantiatedStrength = Instantiate(StrengthPrefab);

        GameObject instantiatedProcessor = Instantiate(processorPrefab);
        instantiatedProcessor.GetComponent<InterjectionProcessor>().startingValue = Mathf.FloorToInt(StrengthAmount + (StrengthScaling * transform.root.GetComponent<Enemy>().difficultyModifier - 1));
        interjectionEvent.Raise(new CallForInterjections(gameObject, transform.root.gameObject, InteractionType.Status, instantiatedProcessor.GetComponent<InterjectionProcessor>()));

        instantiatedStrength.GetComponent<Status>().value = instantiatedProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();

        transform.root.GetComponent<ITakeStatus>().TakeStatus(gameObject, instantiatedStrength);

        Destroy(instantiatedProcessor);
    }
}
