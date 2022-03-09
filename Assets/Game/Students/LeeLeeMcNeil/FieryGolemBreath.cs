using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class FieryGolemBreath : EnemyAction, ITargetPlayer
{
    public int damage;
    public GameObject playerTargeterPrefab;
    GameObject instantiatedTargeter;
    GameObject playerTarget;

    public GameObject burnCardPrefab;
    public int numberOfBurnCards;

    public GameObjectGameEvent drawCardEvent;

    public GameObject scorchPrefab;
    public int initialScorchValue;

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

        for (int index = 0; index < numberOfBurnCards; index++)
        {
            GameObject instantiatedBurn = Instantiate(burnCardPrefab);
            playerTarget.GetComponentInChildren<DeckManager>().AddCardToDeck(instantiatedBurn);
            drawCardEvent.Raise(instantiatedBurn);
        }

        GameObject instantiatedStatus = Instantiate(scorchPrefab);
        GameObject statusProcessor = Instantiate(processorPrefab);
        statusProcessor.GetComponent<InterjectionProcessor>().startingValue = initialScorchValue;
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, playerTarget, InteractionType.Status, damageProcessor.GetComponent<InterjectionProcessor>()));
        instantiatedStatus.GetComponent<Status>().value = statusProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
        playerTarget.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedStatus);
        Destroy(damageProcessor);
    }
}