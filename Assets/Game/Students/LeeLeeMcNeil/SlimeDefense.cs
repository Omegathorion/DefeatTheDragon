using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class SlimeDefense : Card, ITargetPlayer
{

    public GameObject targeterPrefab;
    private GameObject currentTargeter;
    public GameObject target;

    public GameObject processorPrefab;
    private GameObject currentProcessor;
    public CallForInterjectionsGameEvent interjectionEvent;
    public GameObjectGameEvent onCardPlayed;
    public GameObject blockinitial;
    public GameObject SlimeResidue;
    public int SlimeAmount;
    public int BlockAmount;
    
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
                GameObject blockPrefab = Instantiate(blockinitial);
                slimeProcessor.GetComponent<InterjectionProcessor>().startingValue = SlimeAmount;
                interjectionEvent.Raise(new CallForInterjections(this.gameObject, target, InteractionType.Status, slimeProcessor.GetComponent<InterjectionProcessor>()));
                instantiatedSlime.GetComponent<Status>().value = slimeProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
                Destroy(slimeProcessor);
                target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedSlime);
                onCardPlayed.Raise(this.gameObject);
                interjectionEvent.Raise(new CallForInterjections(this.gameObject, target, InteractionType.Block, currentProcessor.GetComponent<InterjectionProcessor>()));
                blockPrefab.GetComponent<Status>().value = currentProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
                target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, blockPrefab);
                Destroy(currentProcessor);

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
            currentProcessor.GetComponent<InterjectionProcessor>().startingValue = BlockAmount;
            CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, target, InteractionType.Block, currentProcessor.GetComponent<InterjectionProcessor>());
            interjectionEvent.Raise(currentInterjection);

        }
    }
}
