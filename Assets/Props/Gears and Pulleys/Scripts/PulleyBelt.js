#pragma strict

@script RequireComponent(ScrollTexture);

var pulleyRigidbodyA : Rigidbody;
var pulleyRigidbodyB : Rigidbody;

var torqueTransferRatio : float = 1.0;
var torqueTransferMultiplier : float = 1.0;
var drag : float = 1.0;

private	var pulleyASpeed : float;
private	var pulleyBSpeed : float;

//Scroll texture.
private var scrollTexture : ScrollTexture;
private var scrollSpeed : float;
var scrollMultiplier : float = 1.0;


//Belt oscillation.
private var beltOscillations : OscillateRotation [];
private var currentGlobalBeltOscillation : float;
private var globalBeltOscillationVelocity : float;
var beltOscillationMultiplier : float = 1.0;

function Start () {
	//Scroll texture.
	scrollTexture = GetComponent.<ScrollTexture>();

	//Belt oscillation.
	beltOscillations = gameObject.GetComponentsInChildren.<OscillateRotation>();
	for(var beltOscillation : OscillateRotation in beltOscillations){
		beltOscillation.frequency = 10.0 + Random.value * 5.0;
		beltOscillation.oscillation = Vector3.zero;
	}
}

function Update () {
	//Scroll speed.
	pulleyASpeed = transform.InverseTransformDirection(pulleyRigidbodyA.angularVelocity).y;
	pulleyBSpeed = transform.InverseTransformDirection(pulleyRigidbodyB.angularVelocity).y;
	var targetScrollSpeed : float = pulleyASpeed + pulleyBSpeed;
	scrollSpeed = (pulleyASpeed * torqueTransferRatio + pulleyBSpeed * (1/torqueTransferRatio)) * .5;
	scrollTexture.scrollUV.x = scrollSpeed;	
	
	//Belt oscillation.
	currentGlobalBeltOscillation = Mathf.SmoothDamp(currentGlobalBeltOscillation, scrollSpeed, globalBeltOscillationVelocity, .5);
	for(var beltOsillation : OscillateRotation in beltOscillations){
		beltOsillation.oscillation.x = currentGlobalBeltOscillation * beltOscillationMultiplier;
		beltOsillation.oscillation.y = currentGlobalBeltOscillation * beltOscillationMultiplier;
		beltOsillation.oscillation.z = currentGlobalBeltOscillation * beltOscillationMultiplier;
	}	
}

function FixedUpdate(){
	pulleyRigidbodyA.AddTorque(
	transform.TransformDirection(0,pulleyBSpeed - pulleyASpeed *  torqueTransferRatio   , 0) * torqueTransferMultiplier);
	pulleyRigidbodyB.AddTorque(
	transform.TransformDirection(0,pulleyASpeed - pulleyBSpeed * (1/torqueTransferRatio), 0) * torqueTransferMultiplier);
}