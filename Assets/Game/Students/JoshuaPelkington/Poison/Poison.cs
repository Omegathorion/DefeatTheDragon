public class Poison : Status
{
    public void ReceiveInterjectionCall(CallForInterjections receivedCall)
    {
        if (receivedCall.target == statusOwner)
        {
            //Not Yet Implemented
            if (receivedCall.typeOfInteraction == InteractionType.Status)
            {

            }
        }
    }
}