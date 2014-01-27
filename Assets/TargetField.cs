using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List

public class TargetField : MonoBehaviour {

	private bool hasTarget = false;
	[SerializeField]
	private Transform playerTranform;
	private Transform lockedTarget;

	// List of targets that this collider collides with
	[SerializeField]
	private Dictionary<int, Transform> targets;
	[SerializeField]
	private int keyCount = 0;

	// Use this for initialization
	void Start () {
		targets = new Dictionary<int, Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		// DEBUG
		//Debug.Log(targets.Count);
		foreach(Transform t in targets.Values) {
			Debug.DrawLine(playerTranform.position, t.position, Color.red);
		}
		// END DEBUG

		if (hasTarget)
		{

		}
		else 
		{
			// We need to 
		}
	}

	void OnTriggerEnter(Collider other) {
		// Check if the collider is an Enemy
		if (other.tag == "Enemy") {
			// Get script
			EnemyAi enemy = other.GetComponent<EnemyAi>() as EnemyAi;
			// If enemy was not already in target field, add them to the list of targets
			if (!enemy.IsInTargetField()) {
				// Add to dict
				targets.Add(keyCount, other.transform);
				// Set index in list for retrieval, mapping
				enemy.SetTargetKey(keyCount++);
			}
		}
	}

	// POSSIBLE PERFORMANCE ENHANCEMENT: Maybe do this in update by checking each collider in our dictionary with this one, 
	// but this probably won't be needed once layers are implemented
	void OnTriggerExit(Collider other) {
		// Check if the collider is an Enemy
		if (other.tag == "Enemy") {
			EnemyAi enemy = other.GetComponent<EnemyAi>() as EnemyAi;
			// Set InTargetField to false so that we can add it again later if we need to
			enemy.SetInTargetField(false);
			// Remove this enemy entry from our dictionary
			targets.Remove(enemy.GetTargetKey());
			// If this made our dictionary empty, then set the count back to zero
			// This may not be necessary but whatever
			if (targets.Count == 0 )
				keyCount = 0;
		}
	}

	// Should be called as soon as the player presses the lock Target Button
	public Transform GetClosetTarget() {
		float shortestDistance = Mathf.Infinity;
		foreach(Transform t in targets.Values) {
			float distance = Vector3.Distance(playerTranform.position, t.position);
			if (distance < shortestDistance) {
				shortestDistance = distance;
				lockedTarget = t;
			}
		}
		if (lockedTarget != null)
			Debug.DrawLine(playerTranform.position, lockedTarget.position, Color.blue);
		Debug.Log(lockedTarget);
		return lockedTarget;
	}

	// @return returns true if a new target gets acquired, false otherwise
	// Given some input from the controller, find a target in that direction from current target
	public bool SwitchTarget(float directionX, float directionY) {
		return false;
	}

	// This gets called whenever the target button comes up
	public void Reset () {
		lockedTarget = null;
		hasTarget = false;
	}
}
