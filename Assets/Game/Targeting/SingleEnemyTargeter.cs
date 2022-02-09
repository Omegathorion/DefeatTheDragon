using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEnemyTargeter : Targeter
{
    public LineRenderer myLine;
    public LayerMask enemyLayer;

    void Update()
    {
        transform.position = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        myLine.SetPosition(0, requester.transform.position);
        myLine.SetPosition(1, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            requester.GetComponent<ITargetSingleEnemy>().ReceiveSingleEnemyTarget(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        requester.GetComponent<ITargetSingleEnemy>().ReceiveSingleEnemyTarget(null);
    }
}
