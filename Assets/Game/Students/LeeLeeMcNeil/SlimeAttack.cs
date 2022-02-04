using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class SlimeAttack : Card, ITargetSingleEnemy
{
    
    public GameObject targeterPrefab;
    private GameObject currentTargeter;
    public GameObject target;

    public GameObject processorPrefab;
    private GameObject currentProcessor;
    public CallForInterjectionsGameEvent interjectionEvent;

    public GameObject SlimeResidue;
    public int SlimeAmount;

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
                
                playerMana.Value -= processedManaCost;
                discardCardEvent.Raise(this.gameObject);

                GameObject instantiatedSlime = Instantiate(SlimeResidue);
                GameObject slimeProcessor = Instantiate(processorPrefab);
                slimeProcessor.GetComponent<InterjectionProcessor>().startingValue = SlimeAmount;
                interjectionEvent.Raise(new CallForInterjections(this.gameObject, target, InteractionType.Status, slimeProcessor.GetComponent<InterjectionProcessor>()));
                instantiatedSlime.GetComponent<Status>().duration = slimeProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
                Destroy(slimeProcessor);
                target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedSlime);
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
       
    }
}
