#pragma strict

var pair : Transform;

var offset : float;

private var pairAngularVelocity : float;
private var gearAngularVelocity : float;

function FixedUpdate () {
	pairAngularVelocity = pair.rigidbody.angularVelocity.y;
	gearAngularVelocity = rigidbody.angularVelocity.z;
	
	var localRotation : Quaternion = transform.localRotation;
	var targetRotation : Quaternion;
	transform.localRotation = (Quaternion.Euler(pair.localEulerAngles.z, pair.localEulerAngles.x, pair.localEulerAngles.y));
	targetRotation = transform.rotation;
	transform.localRotation = localRotation;
	//rigidbody.MoveRotation(targetRotation);	

	
	pair.rigidbody.AddTorque(pair.TransformDirection(0,gearAngularVelocity * 3.0, 0));
			
}