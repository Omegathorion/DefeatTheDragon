using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class PoisonTick : Card, ITargetAllEnemies // targeter for all enemies? 
{
    public GameObject processorPrefab;
    
    public GameObjectGameEvent onCardPlayed;
    
    public CallForInterjectionsGameEvent interjectionEvent;

    
    public GameObject targeterPrefab;
    private GameObject currentTargeter;

    public GameObject poisonPrefab;

    public List<GameObject> targets = new List<GameObject>();
    
    public int PoisonAmount;
    private List<GameObject> handlist = new List<GameObject>();
    private GameObject cardInUse;
    public int DiscardAmt;
    private int randomDiscardnum;
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
               discardCardEvent.Raise(this.gameObject);
              foreach (GameObject target in targets)
                {
                    GameObject instantiatedPoison = Instantiate(poisonPrefab);

                    GameObject poisonProcessor = Instantiate(processorPrefab);
                    poisonProcessor.GetComponent<InterjectionProcessor>().startingValue = PoisonAmount;
                    interjectionEvent.Raise(new CallForInterjections(this.gameObject, target, InteractionType.Status, poisonProcessor.GetComponent<InterjectionProcessor>()));
                    instantiatedPoison.GetComponent<Status>().value = poisonProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
                    Destroy(poisonProcessor);
                    target.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedPoison);
                }
              for (int Index = 0; Index < DiscardAmt; Index ++)
                {
                    DiscardRandomCard();
                }
                
                onCardPlayed.Raise(this.gameObject);
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
      
        currentTargeter = null;

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

    //Also do we have a targeter for all enemies now? 
    public void ReceiveAllEnemyTargets(List<GameObject> receivedEnemies)
    {
        targets = receivedEnemies;
        

    }

    public void DiscardRandomCard()
    {
        handlist.Clear();
        foreach (Card cardInUse in this.transform.root.GetComponentInChildren<DeckManager>().hand.GetComponentsInChildren<Card>())
        {
            handlist.Add(cardInUse.gameObject);
            
        }
        if (handlist.Count > 0)
        {
            randomDiscardnum = Random.Range(0, handlist.Count);
            discardCardEvent.Raise(handlist[randomDiscardnum]);
        }
        


}
}
