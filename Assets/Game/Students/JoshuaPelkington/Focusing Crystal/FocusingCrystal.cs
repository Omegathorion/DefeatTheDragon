using UnityEngine;

public class FocusingCrystal : Relic
{
    GameObject player;

    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    public void RecieveStatusInterjection(CallForInterjections receivedCall)
    {
        if (receivedCall.target != player)
        {
            if (receivedCall.typeOfInteraction == InteractionType.Status)
            {
                receivedCall.processor.ReceiveAdder(gameObject, 1);
            }
        }
    }
}
