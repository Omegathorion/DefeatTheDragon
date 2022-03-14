using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionStrike : Card, ITargetSingleEnemy
{
    public GameObject targeterPrefab;
    private GameObject currentTargeter;
    public GameObject target;

    public GameObject processorPrefab;
    private GameObject currentProcessor;
    public CallForInterjectionsGameEvent interjectionEvent;

    public GameObject vulnerabilityPrefab;
    public int vulnerabilityAmount;
    public int damage;

    public override void OnMouseUp()
    {
        if (target != null)
        {
            GameObject manaProcessor = Instantiate(processorPrefab);
            manaProcessor.GetComponent<InterjectionProcessor>().startingValue = manaCost;
            interjectionEvent.Raise(new CallForInterjections(this.gameObject, this.gameObject, InteractionType.Mana, manaProcessor.GetComponent<InterjectionProcessor>()));
            int processedManaCost = manaProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
            if (playerMana >= processedManaCost)
            {
                target.GetComponent<ITakeDamage>().TakeDamage(this.gameObject, currentProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue());
                playerMana.Value -= processedManaCost;
                discardCardEvent.Raise(this.gameObject);
            }
            Destroy(manaProcessor);
        }
        Deinitiate();
    }

    public void Deinitiate()
    {
        if (currentTargeter != null)
        {
            Destroy(currentTargeter);
        }
        if (currentProcessor != null)
        {
            Destroy(currentProcessor);
        }
        currentTargeter = null;
        currentProcessor = null;
        target = null;
    }

    public override void OnMouseDown()
    {
        base.OnMouseDown();
        if (currentTargeter == null)
        {
            currentTargeter = GameObject.Instantiate(targeterPrefab);
            currentTargeter.GetComponent<Targeter>().requester = this.gameObject;
        }
    }

    public void ReceiveSingleEnemyTarget(GameObject receivedTarget)
    {
        target = receivedTarget;
        if (receivedTarget == null)
        {
            if (currentProcessor != null)
            {
                Destroy(currentProcessor);
            }
        }
        else
        {
            currentProcessor = GameObject.Instantiate(processorPrefab);
            currentProcessor.GetComponent<InterjectionProcessor>().startingValue = damage; // Damage is added by the number of cards you played this turn
            CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, target, InteractionType.Damage, currentProcessor.GetComponent<InterjectionProcessor>());
            interjectionEvent.Raise(currentInterjection);

            GameObject instantiatedVulnerability = Instantiate(vulnerabilityPrefab);
            GameObject vulnerabilityProcessor = Instantiate(processorPrefab);
            vulnerabilityProcessor.GetComponent<InterjectionProcessor>().startingValue = vulnerabilityAmount;
            interjectionEvent.Raise(new CallForInterjections(this.gameObject, target, InteractionType.Status, vulnerabilityProcessor.GetComponent<InterjectionProcessor>()));
            instantiatedVulnerability.GetComponent<Status>().value = vulnerabilityProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
            Destroy(vulnerabilityProcessor);
            target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedVulnerability);
        }
    }
}