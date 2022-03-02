using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGremlin : Enemy
{
    public EnemyAction basicAttack;

    public override void Initialize(float receivedDifficultyModifier)
    {
        base.Initialize(receivedDifficultyModifier);
        currentHealth = Mathf.FloorToInt(maxHealth * difficultyModifier);
        UpdateHealthDisplay();
    }

    public override void DecideIntent()
    {
        intendedAction = basicAttack.gameObject;
    }
}
