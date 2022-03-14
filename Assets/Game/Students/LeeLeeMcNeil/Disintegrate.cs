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

    public int DisintegrateDamage;
    private GameObject currentTargeter;

    public int DisintegrateDrawNum;




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
                RemoveBlock(target);
                
                playerMana.Value -= processedManaCost;
                discardCardEvent.Raise(this.gameObject);


                GameObject disintegrateProcessor = Instantiate(processorPrefab);



                disintegrateProcessor.GetComponent<InterjectionProcessor>().startingValue = DisintegrateDamage;
                interjectionEvent.Raise(new CallForInterjections(this.gameObject, target.gameObject, InteractionType.Damage, disintegrateProcessor.GetComponent<InterjectionProcessor>()));
                int FinalDamageDisintegrate = disintegrateProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
                Destroy(disintegrateProcessor);

                target.GetComponent<ITakeDamage>().TakePiercingDamage(this.gameObject, FinalDamageDisintegrate);
                onCardPlayed.Raise(this.gameObject);
                
                DrawCard();
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

    public void RemoveBlock(GameObject receivedTarget)
    {
        Status[] statuses = receivedTarget.GetComponentsInChildren<Status>();
        foreach (Status eachStatus in statuses)
        {

            if (eachStatus.gameObject.GetComponent<Block>())
            {
                Destroy(eachStatus.gameObject);
                break;
            }

        }
    }

    public void DrawCard()
    {
        GameObject drawcardProcessor = Instantiate(processorPrefab);



        drawcardProcessor.GetComponent<InterjectionProcessor>().startingValue = DisintegrateDrawNum;
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, this.transform.root.gameObject, InteractionType.Draw, drawcardProcessor.GetComponent<InterjectionProcessor>()));
        int FinalDrawValue = drawcardProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
        Destroy(drawcardProcessor);
        this.transform.root.GetComponentInChildren<DeckManager>().DrawCardsFromTopOfDeck(FinalDrawValue);

    }
}