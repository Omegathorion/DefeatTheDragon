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
                gameObject.GetComponent<Collider>().enabled = true;
                amITargetingAnythingRightNow = true;
            }
        }
        else
        {
            if (amITargetingAnythingRightNow)
            {
                amITargetingAnythingRightNow = false;
                gameObject.GetComponent<Collider>().enabled = false;
                targets.Clear();
                requester.GetComponent<ITargetAllEnemies>().ReceiveAllEnemyTargets(targets);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            targets.Add(other.gameObject);
            requester.GetComponent<ITargetAllEnemies>().ReceiveAllEnemyTargets(targets);
        }
    }
}
