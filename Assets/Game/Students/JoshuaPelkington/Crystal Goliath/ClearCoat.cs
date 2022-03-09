using UnityEngine;
using DG.Tweening;

public class ClearCoat : EnemyAction, ITargetPlayer
{
    [SerializeField] GameObject playerTargeterPrefab;

    GameObject instantiatedTargeter;
    GameObject playerTarget;

    public override void Execute()
    {
        base.Execute();
        transform.root.DOShakePosition(animationTime, 1, 10, 90, false, true);

        instantiatedTargeter = Instantiate(playerTargeterPrefab);
        instantiatedTargeter.GetComponent<Targeter>().requester = this.gameObject;
    }

    public void ReceivePlayerTarget(GameObject player)
    {
        playerTarget = player;

        Transform playerStatuses = playerTarget.GetComponent<Player>().statuses;

        for (int i = playerStatuses.childCount - 1; i >= 0; i--)
        {
            Destroy(playerStatuses.GetChild(i).gameObject);
        }
    }
}
