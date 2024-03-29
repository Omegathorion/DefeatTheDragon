using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using TMPro;

public class Enemy : MonoBehaviour, ITakeDamage, ITakeStatus
{
    public float difficultyModifier;
    public int currentHealth;
    public int maxHealth;
    public Transform statuses;
    public TextMeshPro healthTextDisplay;
    public GameObject intendedAction;
    public GameObjectGameEvent onDamagedEvent;
    public GameObjectGameEvent onDeathEvent;
	
	[NonSerialized]
	public int prevDamage = 0;


    void Start()
    {
        Initialize(difficultyModifier);
    }

    public virtual void Initialize(float receivedDifficultyModifier)
    {
        difficultyModifier = receivedDifficultyModifier;
    }

    public void UpdateHealthDisplay()
    {
        healthTextDisplay.text = currentHealth.ToString();
    }

    public void TakeDamage(GameObject receivedDamager, int receivedAmount)
    {
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
        if (processingAmount > 0)
        {
            onDamagedEvent.Raise(this.gameObject);
        }
        healthTextDisplay.text = currentHealth.ToString();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakePiercingDamage(GameObject receivedDamager, int receivedAmount)
    {
        currentHealth -= receivedAmount;
        if (receivedAmount > 0)
        {
            onDamagedEvent.Raise(this.gameObject);
        }
        healthTextDisplay.text = currentHealth.ToString();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        onDeathEvent.Raise(this.gameObject);
        Destroy(gameObject);
    }

    public void TakeHealing(GameObject receivedHealer, int receivedAmount)
    {
        currentHealth += receivedAmount;
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

    public virtual void DecideIntent()
    {

    }

    public float ExecuteAction()
    {
        intendedAction.GetComponent<EnemyAction>().Execute();
        return intendedAction.GetComponent<EnemyAction>().animationTime;
    }
}