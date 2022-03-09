using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class CurlUp : Status
{
    public Vector2 blockRange;
    public int blockAmount;
    public GameObject blockPrefab;
    public GameObject processorPrefab;
    public CallForInterjectionsGameEvent interjectionEvent;

    public void OnStartOfCombat()
    {
        blockAmount = Mathf.FloorToInt(Random.Range(blockRange.x, blockRange.y + 1));
    }

    public void IWasDamaged(GameObject whoGotDamaged)
    {
        if (whoGotDamaged = transform.root.gameObject)
        {
            GameObject currentProcessor = GameObject.Instantiate(processorPrefab);
            currentProcessor.GetComponent<InterjectionProcessor>().startingValue = blockAmount;
            CallForInterjections currentInterjection = new CallForInterjections(this.gameObject, transform.root.gameObject, InteractionType.Block, currentProcessor.GetComponent<InterjectionProcessor>());
            interjectionEvent.Raise(currentInterjection);

            GameObject currentBlockObject = Instantiate(blockPrefab);
            currentBlockObject.GetComponent<Block>().value = currentProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
            transform.root.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, currentBlockObject);

            Destroy(currentProcessor);

            Destroy(this.gameObject);
        }
    }
}
