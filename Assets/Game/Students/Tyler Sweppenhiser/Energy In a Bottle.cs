using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EnergyInaBottle : Relic
{
    [Header("Energy")]
    public int EnergyGiftTotal;
   
    public DefaultPlayerManaGainAtStartOfTurn PlayerManaScript;


    public void StartofTurnAditionalMana()
    {

        PlayerManaScript.playerManaVariable.Value += EnergyGiftTotal;
    }

 
}
