#pragma strict

private var defaultPosition : Vector3;

function Start () {
	defaultPosition = transform.position;
}

function Update () {
	transform.position = defaultPosition;
}