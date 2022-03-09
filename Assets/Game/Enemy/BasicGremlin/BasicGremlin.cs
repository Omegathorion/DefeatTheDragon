using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGremlin : Enemy
{
    public EnemyAction basicAttack;

    public override void Initialize(float receivedDifficultyModifier)
    {
        base.Initialize(receivedDifficultyModifier); 
        maxHealth = Mathf.FloorToInt(maxHealth * difficultyModifier);
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public override void DecideIntent()
    {
        intendedAction = basicAttack.gameObject;
    }
}