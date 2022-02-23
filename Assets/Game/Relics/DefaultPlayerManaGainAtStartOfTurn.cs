using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class DefaultPlayerManaGainAtStartOfTurn : MonoBehaviour
{
    public IntVariable playerManaVariable;
    public int amountOfManaThePlayerStartsEachTurnWith;

    public void OnPlayerTurnStart()
    {
        playerManaVariable.Value = amountOfManaThePlayerStartsEachTurnWith;
    }
}