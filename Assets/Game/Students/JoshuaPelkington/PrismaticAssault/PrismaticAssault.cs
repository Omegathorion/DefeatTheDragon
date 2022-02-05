using ScriptableObjectArchitecture;
using UnityEngine;

public class PrismaticAssault : Card, ITargetSingleEnemy
{
    [Header("Damage Settings")]
    public int damage = 2;

    [Header("Status Settings")]
    public GameObject vulnerabilityPrefab;
    public int vulnerabilityAmount = 2;
    public GameObject weaknessPrefab;
    public int weaknessAmount = 2;
    public GameObject poisonPrefab;
    public int poisonAmount = 2;

    [Header("References")]
    public GameObject targeterPrefab;
    private GameObject currentTargeter;
    [HideInInspector]
    public GameObject target;

    public GameObject processorPrefab;
    private GameObject currentProcessor;
    public CallForInterjectionsGameEvent interjectionEvent;

    public override void OnMouseUp()
    {
        if(target != null)
        {
            int manaCost = CheckMana();

            if (playerMana >= manaCost)
            {
                playerMana.Value -= manaCost;

                target.GetComponent<ITakeDamage>().TakeDamage(this.gameObject, currentProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue());
                discardCardEvent.Raise(this.gameObject);

                ApplyStatus(vulnerabilityPrefab, vulnerabilityAmount);
                ApplyStatus(weaknessPrefab, weaknessAmount);
                ApplyStatus(poisonPrefab, poisonAmount);

            }
        }
        Deinitiate();
    }

    int CheckMana()
    {
        GameObject manaProcessor = Instantiate(processorPrefab);
        manaProcessor.GetComponent<InterjectionProcessor>().startingValue = manaCost;
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, this.gameObject, InteractionType.Mana, manaProcessor.GetComponent<InterjectionProcessor>()));
        int processedManaCost = manaProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
        Destroy(manaProcessor);

        return processedManaCost;
    }

    void ApplyDamage(int damageAmount)
    {
        currentProcessor = Instantiate(processorPrefab);
        currentProcessor.GetComponent<InterjectionProcessor>().startingValue = damage;
        CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, target, InteractionType.Damage, currentProcessor.GetComponent<InterjectionProcessor>());
        interjectionEvent.Raise(currentInterjection);
    }

    void ApplyStatus(GameObject statusPrefab, int statusAmount)
    {
        GameObject statusInstance = Instantiate(statusPrefab);
        GameObject statusProcessor = Instantiate(processorPrefab);
        statusProcessor.GetComponent<InterjectionProcessor>().startingValue = statusAmount;
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, target, InteractionType.Status, statusProcessor.GetComponent<InterjectionProcessor>()));
        statusInstance.GetComponent<Status>().duration = statusProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
        Destroy(statusProcessor);
        target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, statusInstance);
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
        else
        {
            ApplyDamage(damage);
        }
    }
}
