#pragma strict

var oscillation : Vector3;
var frequency : float;

private var randomize : Vector3;

private var originalRotation : Quaternion;

function Start () {
	originalRotation = transform.rotation;
	randomize = Random.insideUnitSphere;
}

function Update () {
	transform.rotation = originalRotation;
	
	transform.Rotate(
	Mathf.Sin(Time.time * frequency + randomize.x) * oscillation.x,
	Mathf.Sin(Time.time * frequency + randomize.y) * oscillation.y,
	Mathf.Sin(Time.time * frequency + randomize.z) * oscillation.z);
}