using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashNDasher : Enemy
{
    public EnemyAction slash;
    public EnemyAction dopeUp;
    private int turnCounter = 0;
    public override void Initialize(float receivedDifficultyModifier)
    {
        base.Initialize(receivedDifficultyModifier);
        currentHealth = Mathf.FloorToInt(maxHealth * difficultyModifier);
        UpdateHealthDisplay();
    }

    public override void DecideIntent()
    {
        if (turnCounter >= 1)
        {
            intendedAction = slash.gameObject;
        }

        else if(turnCounter < 1)
        {
            intendedAction = dopeUp.gameObject;
        }

        turnCounter += 1;
    }
}
