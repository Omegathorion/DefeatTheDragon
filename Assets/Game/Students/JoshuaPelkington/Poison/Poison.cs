public class Poison : Status
{
    public override void DecreaseValue()
    {
        statusOwner.GetComponent<ITakeDamage>().TakePiercingDamage(gameObject, value);
        base.DecreaseValue();
    }
}