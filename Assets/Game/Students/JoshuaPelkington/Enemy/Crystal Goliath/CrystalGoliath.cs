using UnityEngine;
using ScriptableObjectArchitecture;

public class CrystalGoliath : Enemy, ITakeDamage, ITakeStatus
{
    [Header("Enemy Actions")]
    [SerializeField] EnemyAction ClearCoat;
    [SerializeField] EnemyAction ShardSlash;
    [SerializeField] EnemyAction CrystalCrush;
    [SerializeField] EnemyAction CorruptCrystalization;

    [Header("Scaling")]
    [SerializeField] float HPScale;

    [Header("Ability")]
    [SerializeField] GameObject PerfectReflectionPrefab;

    int turnCounter = 0;
    bool usedShardSlash = false;
    Transform playerStatuses;

    [Header("Processing")]
    [SerializeField] GameObject processorPrefab;
    [SerializeField] CallForInterjectionsGameEvent interjectionEvent;

    bool PlayerHasStatus { get { return playerStatuses.childCount > 0; } }

    public override void Initialize(float receivedDifficultyModifier)
    {
        base.Initialize(receivedDifficultyModifier);

        GameObject instantiatedPerfectReflectionPrefab = Instantiate(PerfectReflectionPrefab, statuses);
        instantiatedPerfectReflectionPrefab.GetComponent<PerfectReflection>().value = 1;

        playerStatuses = FindObjectOfType<Player>().statuses;

        maxHealth = Mathf.FloorToInt(maxHealth + (maxHealth * (transform.root.GetComponent<Enemy>().difficultyModifier - 1) * HPScale));
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public override void DecideIntent()
    {
        if (turnCounter % 3 == 0)
        {
            usedShardSlash = false;

            if (PlayerHasStatus)
                intendedAction = ClearCoat.gameObject;
            else
                intendedAction = CorruptCrystalization.gameObject;
        }
        else
        {
            if (usedShardSlash)
                intendedAction = CrystalCrush.gameObject;
            else
            {
                intendedAction = ShardSlash.gameObject;
                usedShardSlash = true;
            }
        }

        IncrementTurnCounter();
    }

    void IncrementTurnCounter()
    {
        turnCounter++;
    }

    public new void TakeStatus(GameObject receivedInflicter, GameObject receivedStatus)
    {
        if (statuses.GetComponentInChildren<PerfectReflection>() && receivedInflicter.transform.root.GetComponent<ITakeStatus>() != this)
        {
            GameObject statusProcessor = Instantiate(processorPrefab);
            statusProcessor.GetComponent<InterjectionProcessor>().startingValue = receivedStatus.GetComponent<Status>().value;
            interjectionEvent.Raise(new CallForInterjections(gameObject, receivedInflicter, InteractionType.Status, statusProcessor.GetComponent<InterjectionProcessor>()));
            receivedStatus.GetComponent<Status>().value = statusProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
            receivedInflicter.transform.root.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, receivedStatus);
            Destroy(statusProcessor);
        }
        else
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

}
