using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class DefaultCardDrawAtStartOfTurn : MonoBehaviour
{
    public int amountOfCardsThePlayerDrawsAtStartOfTurn;
    public IntGameEvent requestDrawFromTopOfDeck;
    public GameObject processorPrefab;
    private GameObject currentProcessor;
    public CallForInterjectionsGameEvent callForInterjections;

    public void OnPlayerTurnStart()
    {
        DrawCards();
    }

    public void DrawCards()
    {
        currentProcessor = Instantiate(processorPrefab);
        currentProcessor.GetComponent<InterjectionProcessor>().startingValue = amountOfCardsThePlayerDrawsAtStartOfTurn;
        callForInterjections.Raise(new CallForInterjections(this.gameObject, this.gameObject, InteractionType.Draw, currentProcessor.GetComponent<InterjectionProcessor>()));
        int drawAmount = currentProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
        requestDrawFromTopOfDeck.Raise(drawAmount);
        Destroy(currentProcessor);
    }
}