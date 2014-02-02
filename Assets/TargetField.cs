using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List

public class TargetField : MonoBehaviour {

	private bool hasTarget = false;
	[SerializeField]
	private Transform playerTransform;
	[SerializeField]
	private Transform cameraTransform;
	private ThirdPersonCamera cameraScript;

	private Transform lockedTarget;
	
	// List of targets that this collider collides with
	[SerializeField]
	private Dictionary<int, Transform> targets;
	[SerializeField]
	private int keyCount = 0;

	// 
	private Transform bestTarget;

	// Direction vector of joyStick movement
	Vector3 drawPoint;
	Vector3 stickPoint;
	Vector3 stickDir;

	// Minimum angle between potential target's projected point and locked target for potential target to be considered 
	[SerializeField]
	float minAngle;

	[SerializeField]
	private Transform crosshair;

	// Use this for initialization
	void Start () {
		targets = new Dictionary<int, Transform>();

		cameraScript = cameraTransform.GetComponent<ThirdPersonCamera>();
	}
	
	// Update is called once per frame
	void Update () {
		// DEBUG
		//Debug.Log(targets.Count);
		foreach(Transform t in targets.Values) {
			Debug.DrawLine(playerTransform.position, t.position, Color.grey);
		}
		// END DEBUG

		if (hasTarget)
		{
			crosshair.transform.position = lockedTarget.position;
		}
		else 
		{
			// We need to 
		}
	}

	/*				OnTriggerEvent
	 * 
	 *  When an enemy or object enters the target field:
	 * 		1. add their transform to Dictionary of targets (key, tranform)
	 * 		2. SetTheEnemies local targetKey variable to 1 + the key count
	 */
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
				enemy.SetInTargetField(true);
			}
		}
	}

	/*				OnTriggerExit
	 * 
	 * When an enemy or object leaves the target field:
	 * 		1. Remove the enemy from our dictionary by grabbing it's local variable targetKey
	 * 		2. 
	 * 
	 */
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

	/* 
	 * 					GetClosestTarget()
	 * 
	 * 	Should be called as soon as the player presses the lock Target Button
	 *  Returns the closest target inside the target field to the player
	 */
	public Transform GetClosetTarget() {
		float shortestDistance = Mathf.Infinity;
		foreach(Transform t in targets.Values) {
			float distance = Vector3.Distance(playerTransform.position, t.position);
			if (distance < shortestDistance) {
				shortestDistance = distance;
				lockedTarget = t;
			}
		}
		if (lockedTarget != null) {
			Debug.DrawLine(playerTransform.position, lockedTarget.position, Color.blue);
		
			// Make camera lookAt this target
			cameraScript.setLookAt(lockedTarget.position);
			hasTarget = true;
		}
		Debug.Log(lockedTarget);
		return lockedTarget;
	}

	float stickX;
	float stickY;
	Vector3 normal;

	// @return returns true if a new target gets acquired, false otherwise
	// Given some input from the controller, find a target in that direction from current target
	public Transform SwitchTarget(float rightX, float rightY) {

		stickX = rightX;
		stickY = rightY;

		Debug.Log("And the camera transform is......:");
		Debug.Log(cameraTransform.position);

		// PLANE for projections. 
		// NORMAL FOR PLAYER TO TARGET
		// normal = new Vector3(lockedTarget.position.x - playerTransform.position.x,
		//                             0, lockedTarget.position.z - playerTransform.position.z).normalized; 
		// NORMAL FOR CAMERA TO TARGET
		normal = new Vector3(lockedTarget.position.x - cameraTransform.position.x,
		                     lockedTarget.position.y - cameraTransform.position.y, 
		                     lockedTarget.position.z - cameraTransform.position.z).normalized; 	// Gives a normal 

		Debug.DrawRay(lockedTarget.position, normal*5, Color.black, 5f);

		// Point made by joystick movement, converted to lockedTargetSpace
		// We actually need this to be based on the camera's position
		Debug.Log("RightX: " + rightX + "  RightY: " + rightY);
		//Vector3 drawPoint = playerTransform.right * 2 * rightX + playerTransform.up * 2 * rightY;
		drawPoint = cameraTransform.right * rightX + cameraTransform.up * rightY;
		stickPoint = lockedTarget.position - drawPoint;
		Debug.DrawLine(lockedTarget.position, stickPoint, Color.blue, 5.0f);

		// Direction vector from target to stick direction
		stickDir = lockedTarget.position - stickPoint;

		bestTarget = getBestTarget(); 
		if (bestTarget == null) 
			return lockedTarget; 
		else {
			lockedTarget = bestTarget;
			cameraScript.setLookAt(bestTarget.position);
		}
		return bestTarget;
	}

	Transform getBestTarget ()
	{
		bestTarget = null;
		float shortestDistance = Mathf.Infinity;

		foreach(Transform t in targets.Values) {
			// Make sure we are not using the current target as a potential target
			if (lockedTarget.GetComponent<EnemyAi>().GetTargetKey() == t.GetComponent<EnemyAi>().GetTargetKey() )
				continue;

			// Between locked target and potential target
			Vector3 potentialToLocked = t.position - lockedTarget.position;
			//Debug.DrawLine(t.position, lockedTarget.position, Color.white, 5f);

			// Multiply component by component to normal
			Vector3 dist = Vector3.Scale(potentialToLocked, normal);
			dist = dist.x * normal + dist.y * normal + dist.z * normal;

			// Projected Point
			Vector3 projectedPoint = t.position - dist;
			Debug.DrawLine(t.position, projectedPoint, Color.green, 5f);			                                            

			// Draw line from projected point to locked target
			Vector3 projectedToLocked = projectedPoint - lockedTarget.position;
			Debug.DrawLine(projectedPoint, lockedTarget.position, Color.grey, 5f);

			// Get angle between joystick direction and projected point
			Debug.DrawLine(projectedPoint, stickPoint, Color.black, 10f);
			float angle = Vector3.Angle(projectedToLocked, stickDir); 
			Debug.Log("Angle: " + angle);
			if (angle < minAngle) {
				// Filter by closest projected point to target
				Debug.Log("XY Distance: " +  Vector3.Magnitude(lockedTarget.position - t.position));
				// if (Vector3.Magnitude(projectedToLocked) < shortestDistance)
				if (Vector3.Magnitude(potentialToLocked) < shortestDistance) 
				{
					bestTarget = t;
					shortestDistance = Vector3.Magnitude(lockedTarget.position - t.position);
				}
			}
		}
		return bestTarget;
	}

	// This gets called whenever the target button comes up
	public void Reset () {
		lockedTarget = null;
		hasTarget = false;
	}
}
