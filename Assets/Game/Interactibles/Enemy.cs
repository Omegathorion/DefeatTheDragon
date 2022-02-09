using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Enemy : MonoBehaviour, ITakeDamage, ITakeStatus
{
    public int currentHealth;
    public int maxHealth;
    public Transform statuses;

    public void TakeDamage(GameObject receivedDamager, int receivedAmount)
    {
        int processingAmount = receivedAmount;
        if (statuses.GetComponentInChildren<Block>())
        {
            Block currentBlock = statuses.GetComponentInChildren<Block>();
            int currentBlockAmount = processingAmount;
            if (processingAmount > currentBlock.value)
            {
                currentBlockAmount = currentBlock.value;
            }
            processingAmount -= currentBlockAmount;
            currentBlock.value -= currentBlockAmount;
        }

        currentHealth -= processingAmount;
    }

    public void TakePiercingDamage(GameObject receivedDamager, int receivedAmount)
    {
        currentHealth -= receivedAmount;
    }

    public void TakeStatus(GameObject receivedInflicter, GameObject receivedStatus)
    {
        //Check to see if this status already exists
        bool doesThisStatusAlreadyExist = false;
        foreach (Transform eachStatus in statuses.GetComponentsInChildren<Transform>())
        {
            if (eachStatus.gameObject.name == receivedStatus.name)
            {
                doesThisStatusAlreadyExist = true;
                eachStatus.GetComponent<Status>().value += receivedStatus.GetComponent<Status>().value;
                eachStatus.GetComponent<Status>().onStatusUpdated.Raise(eachStatus.gameObject);
                Destroy(receivedStatus);
            }
        }
        if (doesThisStatusAlreadyExist == false)
        {
            receivedStatus.transform.parent = statuses;
            receivedStatus.GetComponent<Status>().statusOwner = this.gameObject;
            receivedStatus.GetComponent<Status>().onStatusApplied.Raise(receivedStatus);
        }
    }
}