using UnityEngine;
using System.Collections;

public class OnCollideMakeParent : MonoBehaviour {
	
	// Update is called once per frame
	void OnCollisionEnter (Collision collision) {
		if (collision.transform.tag.Equals("Player") )
	    {
			Debug.Log("Player is parented");
			collision.transform.parent = this.transform;
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.transform.tag.Equals("Player") )
		{
			Debug.Log("Player is parented");
			collision.transform.parent = null;
		}
	}
}
