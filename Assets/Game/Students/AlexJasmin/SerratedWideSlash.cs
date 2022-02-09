using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class SerratedWideSlash : Card, ITargetAllEnemies
{
    public int damage;
    public GameObject targeterPrefab;
    private GameObject currentTargeter;
    public List<GameObject> targets = new List<GameObject>();

    public GameObject processorPrefab;
    private List<GameObject> currentProcessors = new List<GameObject>();
    public CallForInterjectionsGameEvent interjectionEvent;

    public GameObject weakenPrefab;
    public int weakenAmount;

    public override void OnMouseUp()
    {
        if (targets.Count == 0)
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

                foreach (GameObject target in targets)
                {
                    target.GetComponent<ITakeDamage>().TakeDamage(this.gameObject, currentProcessors[targets.IndexOf(target)].GetComponent<InterjectionProcessor>().CalculateFinalValue());

                    GameObject instantiatedweakening = Instantiate(weakenPrefab);
                    GameObject weakeningProcessor = Instantiate(processorPrefab);
                    weakeningProcessor.GetComponent<InterjectionProcessor>().startingValue = weakenAmount;
                    interjectionEvent.Raise(new CallForInterjections(this.gameObject, target, InteractionType.Status, weakeningProcessor.GetComponent<InterjectionProcessor>()));
                    instantiatedweakening.GetComponent<Status>().duration = weakeningProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
                    Destroy(weakeningProcessor);
                    target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedweakening);
                }
                cardPlayedEvent.Raise(gameObject);
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
        if (currentProcessors.Count != 0)
        {
            foreach (GameObject eachProcessor in currentProcessors)
            {
                Destroy(eachProcessor);
            }
        }
        currentTargeter = null;
        currentProcessors.Clear();
        targets.Clear();
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

    /*public void ReceiveSingleEnemyTarget(GameObject receivedTarget)
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
    }*/

    public void ReceiveAllEnemyTargets(List<GameObject> receivedTargets)
    {
        targets = receivedTargets;
        if (receivedTargets.Count == 0)
        {
            if (currentProcessors.Count != 0)
            {
                foreach (GameObject eachProcessor in currentProcessors)
                {
                    Destroy(eachProcessor);
                }
                currentProcessors.Clear();
            }
        }
        else
        {
            if (currentProcessors.Count != 0)
            {
                foreach (GameObject eachProcessor in currentProcessors)
                {
                    Destroy(eachProcessor);
                }
                currentProcessors.Clear();
            }
            foreach (GameObject eachTarget in receivedTargets)
            {
                GameObject currentProcessor = GameObject.Instantiate(processorPrefab);
                currentProcessor.GetComponent<InterjectionProcessor>().startingValue = damage;
                CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, eachTarget, InteractionType.Damage, currentProcessor.GetComponent<InterjectionProcessor>());
                interjectionEvent.Raise(currentInterjection);
                currentProcessors.Add(currentProcessor);
            }
        }
    }
}
