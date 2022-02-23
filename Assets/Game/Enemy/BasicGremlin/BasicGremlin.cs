using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGremlin : Enemy
{
    public EnemyAction basicAttack;
    public EnemyAction basicAttack2;

    public override void DecideIntent()
    {
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            intendedAction = basicAttack.gameObject;
        }
        else
        {
            intendedAction = basicAttack2.gameObject;
        }
    }
}
