using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieryGolem : Enemy
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
    //Applies 3 burns 1 wound along with a normal hit attack
}
