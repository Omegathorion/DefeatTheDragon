using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class BloodStrength : Relic {
	
	[Header("Strength")]
	public int strengthToIncrease = 2;
	public GameObject statusPrefab;
	public GameObject processorPrefab;
	public CallForInterjectionsGameEvent interjectionEvent;

	// Currently I have the function below called on the start of the player turn since there isn't an event to call when an enemy is defeated yet.	
	public void WhenEnemyIsDefeated() {
		Player player = transform.root.GetComponent<Player>();

		GameObject instantiatedStatus = Instantiate(statusPrefab);
		GameObject instantiatedProcessor = Instantiate(processorPrefab);
		instantiatedProcessor.GetComponent<InterjectionProcessor>().startingValue = strengthToIncrease;
		interjectionEvent.Raise(new CallForInterjections(this.gameObject, transform.root.gameObject, InteractionType.Status, instantiatedProcessor.GetComponent<InterjectionProcessor>()));
		instantiatedStatus.GetComponent<Status>().value = instantiatedProcessor.GetComponent<InterjectionProcessor>().CalculateFinalValue();
		transform.root.GetComponent<ITakeStatus>().TakeStatus(this.gameObject, instantiatedStatus);
		Destroy(instantiatedProcessor);
	}
}
