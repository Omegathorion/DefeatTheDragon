using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class PlayerTargeter : Targeter
{
    public CardPositioningVariable theCardPositionVariable;
    bool amITargetingAnythingRightNow = false;
    public LayerMask playerLayer;
    public GameObject target;

    void Update()
    {
        transform.position = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (transform.position.y > theCardPositionVariable.Value.yPosition)
        {
            if (!amITargetingAnythingRightNow)
            {
                DetectPlayer();
                amITargetingAnythingRightNow = true;
            }
        }
        else
        {
            if (amITargetingAnythingRightNow)
            {
                amITargetingAnythingRightNow = false;
                target = null;
                requester.GetComponent<ITargetPlayer>().ReceivePlayerTarget(target);
            }
        }
    }

    private void DetectPlayer()
    {
        Collider[] allHits = Physics.OverlapBox(Vector3.zero, Vector3.one * 100.0f, Quaternion.identity, playerLayer, QueryTriggerInteraction.Collide);
        target = allHits[0].gameObject;
        requester.GetComponent<ITargetPlayer>().ReceivePlayerTarget(target);
    }
}
