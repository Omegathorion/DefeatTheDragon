using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rager : Enemy
{
    public RagerAttack basicAttack;

    public override void Initialize(float receivedDifficultyModifier)
    {
        base.Initialize(receivedDifficultyModifier);
        maxHealth = Mathf.FloorToInt(maxHealth * difficultyModifier);
        currentHealth = maxHealth;
		this.basicAttack.baseDamage = Mathf.FloorToInt(Mathf.Clamp(this.basicAttack.baseDamage * difficultyModifier, 2, 6));
        UpdateHealthDisplay();
    }

    public override void DecideIntent()
    {
        intendedAction = basicAttack.gameObject;
    }
}