using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentinel : Enemy
{
    public int damagePerCard;
    public int cardsPlayedThisTurn;

    public LanceStrike lanceStrike;
    public EnemyAction basicAttack;
    public EnemyAction healSelf;

    public GameObjectGameEvent cardPlayedEvent;

    public bool shouldAttack = true;

    public override void Initialize(float receivedDifficultyModifier)
    {
        base.Initialize(receivedDifficultyModifier);
        currentHealth = Mathf.FloorToInt(maxHealth * difficultyModifier);
        UpdateHealthDisplay();
        lanceStrike.sentinel = this;
    }

    public override void DecideIntent()
    {
        if (shouldAttack)
        {
            intendedAction = basicAttack.gameObject;
            shouldAttack = false;
        }
        else
        {
            intendedAction = healSelf.gameObject;
            shouldAttack = true;
        }

    }

    public void OnCardPlayed()
    {
        cardsPlayedThisTurn++;
    }

    public void OnTurnStart()
    {
        cardsPlayedThisTurn = 0;
    }
}
