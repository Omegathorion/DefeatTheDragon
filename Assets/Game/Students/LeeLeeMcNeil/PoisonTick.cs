using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class PoisonTick : Card, ITargetSingleEnemy // targeter for all enemies? 
{
    public GameObject processorPrefab;
    
    public GameObjectGameEvent onCardPlayed;
    private GameObject currentProcessor;
    public CallForInterjectionsGameEvent interjectionEvent;

    public GameObject target;
    public GameObject targeterPrefab;
    private GameObject currentTargeter;

    

    
    public int PoisonAmount;

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

                //GameObject instantiatedPoison = Instantiate(); do we already have a poison status? I believe so? Should I call for that?
                GameObject poisonProcessor = Instantiate(processorPrefab);
                poisonProcessor.GetComponent<InterjectionProcessor>().startingValue = PoisonAmount;
                interjectionEvent.Raise(new CallForInterjections(this.gameObject, target, InteractionType.Status, poisonProcessor.GetComponent<InterjectionProcessor>()));
                instantiatedPoison.GetComponent<Status>().value = poisonProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
                Destroy(poisonProcessor);
                target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedPoison);
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

    //Also do we have a targeter for all enemies now? 
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
