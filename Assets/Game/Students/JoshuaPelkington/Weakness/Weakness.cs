using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakness : Status
{
    public float multiplier;

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
}