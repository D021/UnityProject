#pragma strict

var torque : Vector3;
var mode : ForceMode;

function Start () {

}

function FixedUpdate () {
	rigidbody.AddTorque(torque, mode);
}