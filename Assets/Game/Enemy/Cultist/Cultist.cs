using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist : Enemy
{
    public EnemyAction incantationBuff;
    public EnemyAction physicalAttack;
    bool amIBuffed = false;

    public float specialHPScaling;

    public override void Initialize(float receivedDifficultyModifier)
    {
        base.Initialize(receivedDifficultyModifier);

        maxHealth = Mathf.FloorToInt(maxHealth + (maxHealth * (transform.root.GetComponent<Enemy>().difficultyModifier - 1) * specialHPScaling));
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public override void DecideIntent()
    {
        if (!amIBuffed)
        {
            intendedAction = incantationBuff.gameObject;
            amIBuffed = true;
        }
        else
        {
            intendedAction = physicalAttack.gameObject;
        }
    }
}
