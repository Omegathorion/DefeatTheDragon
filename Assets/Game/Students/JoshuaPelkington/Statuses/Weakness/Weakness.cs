using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : Status
{
    public float multiplier;

    bool wasStatusAppliedTheFirstTime = false;

    public void ReceiveInterjectionCall(CallForInterjections receivedCall)
    {
        if (receivedCall.initiator.transform.root.gameObject == statusOwner)
        {
            if (receivedCall.typeOfInteraction == InteractionType.Damage)
            {
                receivedCall.processor.ReceiveMultiplier(this.gameObject, multiplier);
            }
        }
    }

    public override void DecreaseValue()
    {
        if (wasStatusAppliedTheFirstTime)
        {
            wasStatusAppliedTheFirstTime = false;
        }
        else
        {
            base.DecreaseValue();
        }
    }

    public void OnStatusApplied(GameObject receivedStatus)
    {
        if (receivedStatus = this.gameObject)
        {
            wasStatusAppliedTheFirstTime = true;
        }
    }
}