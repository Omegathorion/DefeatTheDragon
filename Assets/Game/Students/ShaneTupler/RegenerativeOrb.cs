using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class RegenerativeOrb : Relic, ITargetPlayer
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
        player.GetComponent<Player>().TakeHealing(gameObject, amountToHeal);
        ResetCardPlayedCount();
        Destroy(instantiatedTargeter);
    }

    public void ResetCardPlayedCount()
    {
        cardsPlayedThisTurn = 0;
    }
}