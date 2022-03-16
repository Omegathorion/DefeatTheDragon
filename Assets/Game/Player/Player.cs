using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using TMPro;

public class Player : MonoBehaviour, ITakeDamage, ITakeStatus
{
    public int currentHealth;
    public int maxHealth;
    public Transform statuses;
    public TextMeshPro healthTextDisplay;
	
	[NonSerialized]
	public bool hasFullBlock = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    void UpdateHealthDisplay()
    {
        healthTextDisplay.text = currentHealth.ToString();
    }

    public void TakeDamage(GameObject receivedDamager, int receivedAmount) {
		
		if (this.hasFullBlock) {
			this.hasFullBlock = false;
			return;
		}
		
		Enemy enemy = receivedDamager.GetComponent<Enemy>();
		if (enemy != null) {
			enemy.prevDamage = receivedAmount;
		}
		
        int processingAmount = receivedAmount;
        if (processingAmount < 0)
        {
            processingAmount = 0;
        }
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
        healthTextDisplay.text = currentHealth.ToString();
    }

    public void TakePiercingDamage(GameObject receivedDamager, int receivedAmount)
    {
		if (this.hasFullBlock) {
			this.hasFullBlock = false;
			return;
		}
		
		Enemy enemy = receivedDamager.GetComponent<Enemy>();
		if (enemy != null) {
			enemy.prevDamage = receivedAmount;
		}
		
        currentHealth -= receivedAmount;
        healthTextDisplay.text = currentHealth.ToString();
    }

    public void TakeHealing(GameObject receivedHealer, int receivedAmount)
    {
        int processingAmount = receivedAmount;

        if (currentHealth + processingAmount > maxHealth)
        {
            processingAmount = maxHealth - currentHealth;
        }

        currentHealth += processingAmount;
        healthTextDisplay.text = currentHealth.ToString();
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