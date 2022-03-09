using UnityEngine;
using DG.Tweening;

public class ShardSlash : EnemyAction, ITargetPlayer
{
    [SerializeField] int Damage;
    [SerializeField] int NumberOfHits;
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

        for (int i = 0; i < NumberOfHits; i++)
        {
            GameObject damageProcessor = Instantiate(processorPrefab);
            damageProcessor.GetComponent<InterjectionProcessor>().startingValue = Mathf.FloorToInt(Damage + transform.root.GetComponent<Enemy>().difficultyModifier);
            interjectionEvent.Raise(new CallForInterjections(this.gameObject, playerTarget, InteractionType.Damage, damageProcessor.GetComponent<InterjectionProcessor>()));
            playerTarget.GetComponent<ITakeDamage>().TakeDamage(this.gameObject, damageProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue());
            Destroy(damageProcessor);
            Destroy(instantiatedTargeter);
        }
    }
}
