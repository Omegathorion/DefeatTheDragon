using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Cheats : MonoBehaviour
{
    public GameEvent startPlayerTurn;
    public IntGameEvent drawCardsEvent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            startPlayerTurn.Raise();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            drawCardsEvent.Raise(5);
        }
    }
}
