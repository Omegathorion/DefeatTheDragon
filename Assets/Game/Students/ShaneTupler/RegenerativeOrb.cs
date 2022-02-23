using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class RegenerativeOrb : Relic
{
    [Header("Healing")]
    public int cardsPlayedThisTurn;
    public int amountToHeal;
    public int thresholdToHeal;

    [Header("Player Targeting")]
    public GameObject playerTargeterPrefab;
    GameObject instantiatedTargeter;

    public void OnCardPlayed()
    {
        cardsPlayedThisTurn++;

        if (cardsPlayedThisTurn == thresholdToHeal)
        {
            HealPlayer();
        }
    }

    public void HealPlayer()
    {
        instantiatedTargeter = Instantiate(playerTargeterPrefab);
        instantiatedTargeter.GetComponent<Targeter>().requester = gameObject;
    }

    public void ReceivePlayerTarget(GameObject player)
    {
        player.GetComponent<Player>().TakeDamage(gameObject, -amountToHeal);
    }
}