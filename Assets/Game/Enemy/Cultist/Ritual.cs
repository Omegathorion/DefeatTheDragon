using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Ritual : Status
{
    public GameObject strengthPrefab;

    public void OnStartOfTurn()
    {
        GameObject instantiatedStrength = Instantiate(strengthPrefab);
        instantiatedStrength.GetComponent<Status>().value = value;
        transform.root.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedStrength);
    }
}
