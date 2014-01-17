#pragma strict

var scrollUV : Vector2;

private var mesh : Mesh;
private var uv : Vector2[];

function Start () {
	mesh = GetComponentInChildren(SkinnedMeshRenderer).sharedMesh;
	mesh = Instantiate(mesh);
	GetComponentInChildren(SkinnedMeshRenderer).sharedMesh = mesh;
	
}

function Update () {
	uv = mesh.uv;
	for(var point : Vector2 in uv){
		point += scrollUV * Time.deltaTime;
	}
	mesh.uv = uv;
}