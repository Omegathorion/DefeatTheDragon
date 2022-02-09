using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class ArmorPierce : Card, ITargetSingleEnemy
{
    public int damage;
    public GameObject targeterPrefab;
    private GameObject currentTargeter;
    public GameObject target;

    public GameObject processorPrefab;
    private GameObject currentProcessor;
    public CallForInterjectionsGameEvent interjectionEvent;

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
                target.GetComponent<ITakeDamage>().TakePiercingDamage(this.gameObject, currentProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue());
                playerMana.Value -= processedManaCost;
                discardCardEvent.Raise(this.gameObject);
                cardPlayedEvent.Raise(this.gameObject);
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
            currentProcessor.GetComponent<InterjectionProcessor>().startingValue = damage;
            CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, target, InteractionType.Damage, currentProcessor.GetComponent<InterjectionProcessor>());
            interjectionEvent.Raise(currentInterjection);
        }
    }
}
