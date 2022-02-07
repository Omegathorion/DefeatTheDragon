using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetAllEnemies
{
    public void ReceiveAllEnemyTargets(List<GameObject> allEnemies);
}
