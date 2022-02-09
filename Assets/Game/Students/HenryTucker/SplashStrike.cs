using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class SplashStrike : Card, ITargetSingleEnemy
{
    public int damage;
    public GameObject targeterPrefab;
    private GameObject currentTargeter;
    public GameObject target;
    [SerializeField] private List<GameObject> secondaryTargets = new List<GameObject>();
    public LayerMask enemyMask;

    public GameObject processorPrefab;
    private GameObject currentProcessor;
    private GameObject currentProcessorLeft;
    private GameObject currentProcessorRight;
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
                target.GetComponent<ITakeDamage>().TakeDamage(this.gameObject, currentProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue());
                for(int index = 0; index<secondaryTargets.Count; index++)
                {
                    if(secondaryTargets[index] != null)
                    {
                        if(index == 0)
                        {
                            secondaryTargets[index].GetComponent<ITakeDamage>().TakeDamage(this.gameObject, currentProcessorLeft.GetComponent<InterjectionProcessor>().CalculateFinalValue());
                        }
                        if(index == 1)
                        {
                            secondaryTargets[index].GetComponent<ITakeDamage>().TakeDamage(this.gameObject, currentProcessorRight.GetComponent<InterjectionProcessor>().CalculateFinalValue());
                        }
                    }
                }
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
        secondaryTargets.Clear();
        if (currentProcessor != null)
        {
            Destroy(currentProcessor);
        }
        if (currentProcessorLeft != null)
        {
            Destroy(currentProcessorLeft);
        }
        if (currentProcessorRight != null)
        {
            Destroy(currentProcessorRight);
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
                if(currentProcessorLeft != null)
                {
                    Destroy(currentProcessorLeft);
                }
                if (currentProcessorRight != null)
                {
                    Destroy(currentProcessorRight);
                }

            }
            secondaryTargets.Clear();
        }
        else
        {
            currentProcessor = GameObject.Instantiate(processorPrefab);
            currentProcessor.GetComponent<InterjectionProcessor>().startingValue = damage;
            CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, target, InteractionType.Damage, currentProcessor.GetComponent<InterjectionProcessor>());
            interjectionEvent.Raise(currentInterjection);
            if(Physics.Raycast(target.transform.position, Vector3.left, out RaycastHit hitInfoLeft, 100, enemyMask, QueryTriggerInteraction.Collide))
            {
                secondaryTargets.Add(hitInfoLeft.collider.gameObject);
                currentProcessorLeft = GameObject.Instantiate(processorPrefab);
                currentProcessorLeft.GetComponent<InterjectionProcessor>().startingValue = damage;
                CallForInterjections currentInterjectionLeft = new CallForInterjections(this.gameObject, hitInfoLeft.collider.gameObject, InteractionType.Damage, currentProcessorLeft.GetComponent<InterjectionProcessor>());
                interjectionEvent.Raise(currentInterjectionLeft);
            }
            else
            {
                secondaryTargets.Add(null);
            }
            if (Physics.Raycast(target.transform.position, Vector3.right, out RaycastHit hitInfoRight, 100, enemyMask, QueryTriggerInteraction.Collide))
            {
                secondaryTargets.Add(hitInfoRight.collider.gameObject);
                currentProcessorRight = GameObject.Instantiate(processorPrefab);
                currentProcessorRight.GetComponent<InterjectionProcessor>().startingValue = damage;
                CallForInterjections currentInterjectionRight = new CallForInterjections(this.gameObject, hitInfoRight.collider.gameObject, InteractionType.Damage, currentProcessorRight.GetComponent<InterjectionProcessor>());
                interjectionEvent.Raise(currentInterjectionRight);
            }
            else
            {
                secondaryTargets.Add(null);
            }
        }
    }
}
