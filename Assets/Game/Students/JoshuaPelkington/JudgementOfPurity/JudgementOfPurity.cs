using ScriptableObjectArchitecture;
using UnityEngine;

public class JudgementOfPurity : Card, ITargetSingleEnemy
{
    [Header("Damage Settings")]
    public int damage = 25;
    public int damageReducedPerStatus = 5;

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

    int CalculateAdjustedDamage(float initialDamage)
    {
        int adjustedDamage = (int)(initialDamage - (target.GetComponent<Enemy>().statuses.childCount * damageReducedPerStatus));
        return Mathf.Clamp(adjustedDamage, 0, int.MaxValue);
    }

    void ApplyDamage(int damageAmount)
    {
        currentProcessor = Instantiate(processorPrefab);
        currentProcessor.GetComponent<InterjectionProcessor>().startingValue = CalculateAdjustedDamage(damage);
        CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, target, InteractionType.Damage, currentProcessor.GetComponent<InterjectionProcessor>());
        interjectionEvent.Raise(currentInterjection);
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
