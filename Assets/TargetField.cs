using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For List

public class TargetField : MonoBehaviour {

	private bool hasTarget = false;
	[SerializeField]
	private Transform playerTransform;
	private Transform lockedTarget;

	// List of targets that this collider collides with
	[SerializeField]
	private Dictionary<int, Transform> targets;
	[SerializeField]
	private int keyCount = 0;

	// List for potential new targets
	private List<Transform> potentialTargets;
	
	// Use this for initialization
	void Start () {
		targets = new Dictionary<int, Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		// DEBUG
		//Debug.Log(targets.Count);
		foreach(Transform t in targets.Values) {
			Debug.DrawLine(playerTransform.position, t.position, Color.red);
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
			float distance = Vector3.Distance(playerTransform.position, t.position);
			if (distance < shortestDistance) {
				shortestDistance = distance;
				lockedTarget = t;
			}
		}
		if (lockedTarget != null)
			Debug.DrawLine(playerTransform.position, lockedTarget.position, Color.blue);
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


		// PLANE for projections. 
		normal = new Vector3(lockedTarget.position.x - playerTransform.position.x,
		                             0, lockedTarget.position.z - playerTransform.position.z).normalized; 	// Gives a normal 
		Debug.DrawRay(lockedTarget.position, normal*5, Color.black, 5f);

		// Point made by joystick movement, converted to lockedTargetSpace
		Debug.Log("RightX: " + rightX + "  RightY: " + rightY);
		Vector3 drawPoint = playerTransform.right * 2 * rightX + playerTransform.up * 2 * rightY;
		drawPoint = lockedTarget.InverseTransformDirection(drawPoint);
		Debug.DrawRay(lockedTarget.position, drawPoint, Color.blue, 5.0f);

		potentialTargets = getPotentialTargets(); 

		if (potentialTargets.Count == 0) { return null; }
		return lockedTarget;
	}

	List<Transform> getPotentialTargets ()
	{
		List<Transform> potTargets = new List<Transform>();

		// Iterate over colliders
		foreach(Transform t in targets.Values) {
			
				// Setting the enemies' world coordinates to local coordinate relative to the selected target
				// Vector3 potentialTarget = lockedTarget.transform.InverseTransformPoint(t.position);

			//Vector3 potentialToLocked = lockedTarget.position - t.position;
			Vector3 potentialToLocked = t.position - lockedTarget.position;
			//Debug.DrawRay(t.position, potentialToLocked, Color.white, 5f);
			Debug.DrawLine(t.position, lockedTarget.position, Color.white, 5f);

			// Multiply component by component
			Vector3 dist = Vector3.Scale(potentialToLocked, normal);
			dist = dist.x * normal + dist.y * normal + dist.z * normal;
			//Debug.DrawLine(lockedTarget.position, Vector3.Scale(normal, dist), Color.blue, 10f); 
			Debug.DrawRay(lockedTarget.position, normal*10, Color.blue, 10f);

			Vector3 projectedPoint = t.position - dist;

			// Rotate point from enemy.forward to playerTranform.forward
			Quaternion rotation = Quaternion.FromToRotation(t.forward, playerTransform.forward);

			Debug.DrawLine(t.position, projectedPoint, Color.green, 5f);
			                                               
			// Move projected point to lockedTarget coodinates
			Debug.Log("Projected Point - X: " + projectedPoint.x +
			          " Y: " + projectedPoint.y +
			          " Z " + projectedPoint.z);

		}
		return potTargets;
	}

	private Vector3 getProjection() {
		return new Vector3(0f,0f,0f);
	}

	// This gets called whenever the target button comes up
	public void Reset () {
		lockedTarget = null;
		hasTarget = false;
	}
}
