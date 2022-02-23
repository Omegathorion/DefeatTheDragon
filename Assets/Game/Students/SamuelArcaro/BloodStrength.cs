using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class BloodStrength : Relic {
	
	[Header("Strength")]
	public int strengthToIncrease = 2;
	
	// Currently I have the function below called on the start of the player turn since there isn't an event to call when an enemy is defeated yet.	
	public void WhenEnemyIsDefeated() {
		Player player = GameObject.Find("Player").GetComponent<Player>();
		// add `strengthToIncrease` by 2
	}
}
