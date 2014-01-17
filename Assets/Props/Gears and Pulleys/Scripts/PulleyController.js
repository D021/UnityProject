#pragma strict

@script RequireComponent(ScrollTexture);

var transferBias : float = 1.0;

var mode : ForceMode;

var pulleyRigidbodyA : Rigidbody;

var pulleyRigidbodyB : Rigidbody;

var torqueTransferMultiplier : float = 1.0;

var drag : float = 0.0;

var beltOscillationMultiplier : float = 1.0;

private var beltOscillations : OscillateRotation [];
private var currentGlobalBeltOscillation : float;
private var globalBeltOscillationVelocity : float;

private var scrollTexture : ScrollTexture;
private var scrollSpeed : float;

private	var pulleyASpeed : float;
private	var pulleyBSpeed : float;

function Start () {
	scrollTexture = GetComponent.<ScrollTexture>();
	
	beltOscillations = gameObject.GetComponentsInChildren.<OscillateRotation>();
	
	for(var beltOscillation : OscillateRotation in beltOscillations){
		beltOscillation.frequency = 10.0 + Random.value * 5.0;
		beltOscillation.oscillation = Vector3.zero;
	}
	
}

function Update () {
	pulleyASpeed = transform.InverseTransformDirection(pulleyRigidbodyA.angularVelocity).y;
	pulleyBSpeed = transform.InverseTransformDirection(pulleyRigidbodyB.angularVelocity).y;
	
	scrollSpeed = Mathf.Lerp(scrollSpeed, (pulleyASpeed * transferBias + pulleyBSpeed * (1/transferBias)) * .5,
	Time.deltaTime * 10);;
	
	scrollSpeed = Mathf.Lerp(scrollSpeed, 0, Time.deltaTime * drag);
	
	scrollTexture.scrollUV.x = scrollSpeed;
	
	currentGlobalBeltOscillation = Mathf.SmoothDamp(currentGlobalBeltOscillation, scrollSpeed, globalBeltOscillationVelocity, .5);
	
	for(var beltOsillation : OscillateRotation in beltOscillations){
		beltOsillation.oscillation.x = currentGlobalBeltOscillation * beltOscillationMultiplier;
		beltOsillation.oscillation.y = currentGlobalBeltOscillation * beltOscillationMultiplier;
		beltOsillation.oscillation.z = currentGlobalBeltOscillation * beltOscillationMultiplier;
	}

}

function FixedUpdate(){
	pulleyRigidbodyA.AddTorque(transform.TransformDirection(0,scrollSpeed * (1/transferBias) - pulleyASpeed,0) * torqueTransferMultiplier, mode);
	pulleyRigidbodyB.AddTorque(transform.TransformDirection(0,scrollSpeed * transferBias - pulleyBSpeed,0) * torqueTransferMultiplier, mode);
}