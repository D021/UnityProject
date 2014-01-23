using UnityEngine;
using System.Collections;

public class CylinderRotate : MonoBehaviour {
	[SerializeField]
	private float speed;
	[SerializeField]
	private float angleThreshold1;
	[SerializeField]
	private float angleThreshold2;

	private Vector3 v;
	private float turn = 1;
	// Use this for initialization
	void Start () {
		v = this.transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {

		if (this.transform.eulerAngles.z < angleThreshold1)
		{
			v.z =+ speed;
		}
		if (this.transform.eulerAngles.z > angleThreshold2)
		{
			v.z =- speed;
		}


		this.transform.Rotate(v * turn);
	}
}
