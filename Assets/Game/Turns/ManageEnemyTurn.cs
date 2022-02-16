using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class ManageEnemyTurn : MonoBehaviour
{
    public LayerMask enemyLayer;
    public List<GameObject> enemies = new List<GameObject>();
    public GameEvent playerTurnStart;

    public void BeginEnemyTurn()
    {
        Collider[] allHits = Physics.OverlapBox(Vector3.zero, Vector3.one * 100.0f, Quaternion.identity, enemyLayer, QueryTriggerInteraction.Collide);
        foreach (Collider eachHit in allHits)
        {
            enemies.Add(eachHit.gameObject);
        }

        StartCoroutine(EnemyActionQueue());
    }

    public IEnumerator EnemyActionQueue()
    {
        foreach (GameObject eachEnemy in enemies)
        {
            float receivedAnimationTime = eachEnemy.GetComponent<Enemy>().ExecuteAction();
            yield return new WaitForSeconds(receivedAnimationTime);
        }
        enemies.Clear();
        playerTurnStart.Raise();
    }
}
