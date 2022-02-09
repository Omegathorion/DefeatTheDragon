using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Status : MonoBehaviour
{
    public int value;
    public GameObject statusOwner;
    public GameObjectGameEvent onStatusApplied;
    public GameObjectGameEvent onStatusUpdated;

    public virtual void MoveToCorrectPosition()
    {

    }

    public virtual void DecreaseValue()
    {
        value -= 1;
        if (value <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void EraseAllValue()
    {
        value = 0;
        if (value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
