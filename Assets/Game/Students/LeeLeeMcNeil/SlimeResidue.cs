using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeResidue : Status
{
    

    public void ReceiveInterjectionCall(CallForInterjections receivedCall)
    {
        if (receivedCall.target == statusOwner)
        {
            if (receivedCall.typeOfInteraction == InteractionType.Damage)
            {

                receivedCall.processor.ReceiveAdder(this.gameObject, -value);

            }
        }
    }
}
