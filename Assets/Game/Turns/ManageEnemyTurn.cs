using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class ManageEnemyTurn : MonoBehaviour
{
    public float enemyActionStartDelay;
    public LayerMask enemyLayer;
    public List<GameObject> enemies = new List<GameObject>();
    public GameEvent playerTurnStart;

    public void BeginEnemyTurn()
    {
        StartCoroutine(EnemyActionQueue());
        Invoke("StartTurn", enemyActionStartDelay + 0.5f);
    }

    public IEnumerator EnemyActionQueue()
    {
        enemies.Clear();
        Collider[] allHits = Physics.OverlapBox(Vector3.zero, Vector3.one * 100.0f, Quaternion.identity, enemyLayer, QueryTriggerInteraction.Collide);
        foreach (Collider eachHit in allHits)
        {
            enemies.Add(eachHit.gameObject);
        }

        yield return new WaitForSeconds(enemyActionStartDelay);
        foreach (GameObject eachEnemy in enemies)
        {
            float receivedAnimationTime = eachEnemy.GetComponent<Enemy>().ExecuteAction();
            yield return new WaitForSeconds(receivedAnimationTime);
        }
        yield return new WaitForSeconds(enemyActionStartDelay);
        playerTurnStart.Raise();
    }

    void StartTurn()
    {
        playerTurnStart.Raise();
    }
}