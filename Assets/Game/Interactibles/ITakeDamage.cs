using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeDamage
{
    public void TakeDamage(GameObject receivedDamager, int receivedAmount);
    public void TakePiercingDamage(GameObject receiveDamager, int receivedAmount);
    public void TakeHealing(GameObject receivedHealer, int receivedAmount);
}