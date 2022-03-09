using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : Card
{
    public int burnDamage;

    public void OnDiscard(GameObject whatWasDiscarded)
    {
        if (whatWasDiscarded == this.gameObject)
        {
            transform.root.GetComponent<ITakeDamage>().TakeDamage(this.gameObject, burnDamage);
        }
    }
}
