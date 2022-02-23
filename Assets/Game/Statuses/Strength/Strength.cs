using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strength : Status
{
    public void ReceiveInterjectionCall(CallForInterjections receivedCall)
    {
        if (receivedCall.initiator.transform.root.gameObject == statusOwner)
        {
            if (receivedCall.typeOfInteraction == InteractionType.Damage)
            {
                receivedCall.processor.ReceiveAdder(this.gameObject, value);
            }
        }
    }
}