using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class AmplificationModule : Relic
{
    GameObject player;
    public GameObject vulnerabilityPrefab;
    public GameObject processorPrefab;
    public CallForInterjectionsGameEvent interjectionEvent;
    public IntVariable playerMana;
    public int vulnerabilityAmount;
    public int manaAmount = 2;
    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    public void AtStartOfTurn()
    {
        playerMana.Value += manaAmount;
        applyVulnerable();
    }

    public void applyVulnerable()
    {
        GameObject instantiatedVulnerability = Instantiate(vulnerabilityPrefab);
        GameObject vulnerabilityProcessor = Instantiate(processorPrefab);
        vulnerabilityProcessor.GetComponent<InterjectionProcessor>().startingValue = vulnerabilityAmount;
        interjectionEvent.Raise(new CallForInterjections(this.gameObject, player, InteractionType.Status, vulnerabilityProcessor.GetComponent<InterjectionProcessor>()));
        instantiatedVulnerability.GetComponent<Status>().value = vulnerabilityProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
        Destroy(vulnerabilityProcessor);
        player.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedVulnerability);
    }
}