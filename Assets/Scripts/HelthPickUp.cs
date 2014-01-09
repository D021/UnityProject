using UnityEngine;
using System.Collections;

public class HelthPickUp : MonoBehaviour {

	[SerializeField]
	private int healthAmount = 50;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision collision)
	{
		Debug.Log("collision w health");
		if (collision.gameObject.tag.Equals("Player") )
		{
			// Get character state script and increment health
			CharacterState script = (CharacterState) collision.transform.GetComponent(typeof(CharacterState));
			script.incrementHealth(healthAmount);
		
			// destroy this object
			Destroy(this.gameObject);
		}
	}
}
