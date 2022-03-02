using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Louse : Enemy
{
    public EnemyAction statusAttack;
    public EnemyAction physicalAttack;
    bool whatShouldIDo;

    public float specialHPScaling;

    public override void Initialize(float receivedDifficultyModifier)
    {
        base.Initialize(receivedDifficultyModifier);
        maxHealth = Mathf.FloorToInt(maxHealth * (difficultyModifier * specialHPScaling));
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public override void DecideIntent()
    {
        whatShouldIDo = !whatShouldIDo;
        if (whatShouldIDo)
        {
            intendedAction = statusAttack.gameObject;
        }
        else
        {
            intendedAction = physicalAttack.gameObject;
        }
    }
}
