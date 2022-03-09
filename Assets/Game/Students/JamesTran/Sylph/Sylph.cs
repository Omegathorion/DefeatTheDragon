using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sylph : Enemy
{
    public EnemyAction pierceAttack;
    public EnemyAction poisonBlock;
    bool intent;

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
        intent = !intent;
        if (intent)
        {
            intendedAction = pierceAttack.gameObject;
        }
        else
        {
            intendedAction = poisonBlock.gameObject;
        }
    }
}
