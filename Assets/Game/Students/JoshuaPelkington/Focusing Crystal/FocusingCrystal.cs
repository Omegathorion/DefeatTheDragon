using UnityEngine;

public class FocusingCrystal : Relic
{
    GameObject player;
    public int boostAmount;

    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    public void RecieveStatusInterjection(CallForInterjections receivedCall)
    {
        if (receivedCall.initiator.transform.root == player.transform)
        {
            if (receivedCall.typeOfInteraction == InteractionType.Status)
            {
                receivedCall.processor.ReceiveAdder(gameObject, boostAmount);
            }
        }
    }
}