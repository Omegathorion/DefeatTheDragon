public class Weakness : Status
{
    public float multiplier = 0.75f;

    public void ReceiveInterjectionCall(CallForInterjections receivedCall)
    {
        if (receivedCall.target == statusOwner)
        {
            //Not Yet Implemented
            if (receivedCall.typeOfInteraction == InteractionType.Status)
            {
                receivedCall.processor.ReceiveMultiplier(this.gameObject, multiplier);
            }
        }
    }
}