using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorch : Status
{
    public void OnPlayerTurnStart()
    {
        transform.root.GetComponent<ITakeDamage>().TakeDamage(this.gameObject, value);
    }
}
