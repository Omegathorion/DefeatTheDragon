using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Sundial : Relic
{
    public IntVariable playerMana;
    public int amountOfManaAdded;
    public int amountOfTimesDeckShufflesBeforeAddingMana;
    private int currentAmountOfShuffles;

    public void WhenTheDeckIsShuffled()
    {
        currentAmountOfShuffles++;
        if (currentAmountOfShuffles >= amountOfTimesDeckShufflesBeforeAddingMana)
        {
            currentAmountOfShuffles = 0;
            playerMana.Value += amountOfManaAdded;
        }
    }
}
