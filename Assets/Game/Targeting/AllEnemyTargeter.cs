using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class AllEnemyTargeter : Targeter
{
    public CardPositioningVariable theCardPositionVariable;
    bool amITargetingAnythingRightNow = false;
    public LayerMask enemyLayer;
    public List<GameObject> targets = new List<GameObject>();

    void Update()
    {
        transform.position = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (transform.position.y > theCardPositionVariable.Value.yPosition)
        {
            if (!amITargetingAnythingRightNow)
            {
                DetectEnemies();
                amITargetingAnythingRightNow = true;
            }
        }
        else
        {
            if (amITargetingAnythingRightNow)
            {
                amITargetingAnythingRightNow = false;
                targets.Clear();
                requester.GetComponent<ITargetAllEnemies>().ReceiveAllEnemyTargets(targets);
            }
        }
    }

    private void DetectEnemies()
    {
        targets.Clear();
        Collider[] allHits = Physics.OverlapBox(Vector3.zero, Vector3.one * 100.0f, Quaternion.identity, enemyLayer, QueryTriggerInteraction.Collide);
        foreach (Collider eachHit in allHits)
        {
            targets.Add(eachHit.gameObject);
        }
        requester.GetComponent<ITargetAllEnemies>().ReceiveAllEnemyTargets(targets);
    }
}
