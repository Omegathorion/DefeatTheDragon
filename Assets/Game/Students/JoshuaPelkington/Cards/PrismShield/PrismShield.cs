using ScriptableObjectArchitecture;
using UnityEngine;

public class PrismShield : Card, ITargetPlayer
{
    [Header("Block Settings")]
    public int blockPerStatus = 4;
    [SerializeField] GameObject blockPrefab;

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
                PlayCard();
            }
        }
        Deinitiate();
    }

    void PlayCard()
    {
        playerMana.Value -= manaCost;

        ApplyBlock(1, blockPerStatus);
        ApplyBlock(target.GetComponent<Player>().statuses.childCount - 1, blockPerStatus);

        cardPlayedEvent.Raise(gameObject);
        discardCardEvent.Raise(gameObject);

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

    void ApplyBlock(int statusCount, int blockAmount)
    {
        for (int i = 0; i < statusCount; i++)
        {
            GameObject currentBlockObject = Instantiate(blockPrefab);
            currentBlockObject.GetComponent<Block>().value = currentProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
            target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, currentBlockObject);
        }
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
            currentProcessor.GetComponent<InterjectionProcessor>().startingValue = blockPerStatus;
            CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, target, InteractionType.Block, currentProcessor.GetComponent<InterjectionProcessor>());
            interjectionEvent.Raise(currentInterjection);
        }
    }
}
