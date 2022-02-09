using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class BattleTrance : Card, ITargetSingleEnemy
{
    public GameObject targeterPrefab;
    private GameObject currentTargeter;
    public GameObject target;

    public GameObject processorPrefab;
    private GameObject currentProcessor;
    public CallForInterjectionsGameEvent interjectionEvent;

    public GameObject preventCardDrawPrefab;
    public int preventCardDrawDuration;
    public int cardDrawAmount;
    public IntGameEvent drawCardsEvent;

    public override void OnMouseUp()
    {
        if (target == null)
        {

        }
        else
        {
            GameObject manaProcessor = Instantiate(processorPrefab);
            manaProcessor.GetComponent<InterjectionProcessor>().startingValue = manaCost;
            interjectionEvent.Raise(new CallForInterjections(this.gameObject, this.gameObject, InteractionType.Mana, manaProcessor.GetComponent<InterjectionProcessor>()));
            int processedManaCost = manaProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
            if (playerMana >= processedManaCost)
            {
                GameObject cardDrawProcessor = Instantiate(processorPrefab);
                cardDrawProcessor.GetComponent<InterjectionProcessor>().startingValue = cardDrawAmount;
                interjectionEvent.Raise(new CallForInterjections(this.gameObject, target, InteractionType.Draw, cardDrawProcessor.GetComponent<InterjectionProcessor>()));
                drawCardsEvent.Raise(cardDrawProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue());
                Destroy(cardDrawProcessor);

                GameObject currentPreventCardDraw = Instantiate(preventCardDrawPrefab);
                currentPreventCardDraw.GetComponent<Status>().duration = currentProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
                target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, currentPreventCardDraw);

                playerMana.Value -= processedManaCost;
                cardPlayedEvent.Raise(this.gameObject);
                discardCardEvent.Raise(this.gameObject);
            }
            else
            {

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
            currentProcessor.GetComponent<InterjectionProcessor>().startingValue = preventCardDrawDuration;
            CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, target, InteractionType.Status, currentProcessor.GetComponent<InterjectionProcessor>());
            interjectionEvent.Raise(currentInterjection);
        }
    }
}