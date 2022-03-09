using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashNDasher : Enemy
{
    public EnemyAction slash;
    public EnemyAction dopeUp;

    private int cardCounter = 0;
    private bool firstTurn = true;
    public override void Initialize(float receivedDifficultyModifier)
    {
        base.Initialize(receivedDifficultyModifier);
        currentHealth = Mathf.FloorToInt(maxHealth * difficultyModifier);
        UpdateHealthDisplay();
    }

    public override void DecideIntent()
    {
        if(firstTurn == true)
        {
            intendedAction = dopeUp.gameObject;
            firstTurn = false;
        }
        else if(firstTurn == false)
        {
            intendedAction = slash.gameObject;
        }
    }
    public void OtherIntent()
    {
        cardCounter += 1;
        if(cardCounter >= 2)
        {
            intendedAction = slash.gameObject;
        }
    }
}
