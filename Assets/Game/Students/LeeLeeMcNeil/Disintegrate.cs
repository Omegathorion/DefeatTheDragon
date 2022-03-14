using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Disintegrate : Card, ITargetSingleEnemy
{
    public GameObject processorPrefab;
    public GameObject target;
    public GameObject targeterPrefab;
    public GameObjectGameEvent onCardPlayed;
    private GameObject currentProcessor;
    public CallForInterjectionsGameEvent interjectionEvent;

    
    private GameObject currentTargeter;
    





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

                
                GameObject disintegrateProcessor = Instantiate(processorPrefab);
                
                interjectionEvent.Raise(new CallForInterjections(this.gameObject, target, InteractionType.Status, disintegrateProcessor.GetComponent<InterjectionProcessor>()));
                instantiatedDisintegrate.GetComponent<Status>().value = disintegrateProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
                Destroy(disintegrateProcessor);
                target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedDisintegrate);
                onCardPlayed.Raise(this.gameObject);
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

    //public void RemoveBlock(GameObject receivedTarget)    This should specifically remove block. Do we need another list for it, like the naughty statuses? 
    //{
        //Status[] statuses = player.GetComponentsInChildren<Status>();
        //foreach (Status eachStatus in statuses)
        //{
            //foreach (GameObject eachNaughtyStatus in naughtyStatuses) 
            //{
                //if (eachStatus.GetType() == eachNaughtyStatus.GetComponent<Status>().GetType())
                //{
                  //  Destroy(eachStatus.gameObject);
                    break;
                //}
           // }
        //}
    }
}
