using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyLight : Card, ITargetPlayer
{
    [Header("Empower")]
    public int cardsPlayedThisTurn;
    public int healingPerCard;


    public GameObject targeterPrefab;
    private GameObject currentTargeter;
    public GameObject target;

    public GameObject processorPrefab;
    private GameObject currentProcessor;
    public CallForInterjectionsGameEvent interjectionEvent;

    public int healAmount;

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
                target.GetComponent<ITakeDamage>().TakeHealing(this.gameObject, healAmount);
                playerMana.Value -= processedManaCost;
                cardPlayedEvent.Raise(this.gameObject);
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

    public void ReceivePlayerTarget(GameObject receivedTarget)
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
            currentProcessor.GetComponent<InterjectionProcessor>().startingValue = healAmount + (cardsPlayedThisTurn * healingPerCard);
            CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, target, InteractionType.Damage, currentProcessor.GetComponent<InterjectionProcessor>());
            interjectionEvent.Raise(currentInterjection);
        }
    }

    public void OnCardPlayed()
    {
        cardsPlayedThisTurn++;
    }

    public void OnTurnStart()
    {
        cardsPlayedThisTurn = 0;
    }
}