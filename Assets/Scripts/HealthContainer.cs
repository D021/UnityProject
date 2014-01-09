using UnityEngine;
using System.Collections;

// Script for the Health container
// on take damage funtion, it gets destroyed and instantiates a pickup items
public class HealthContainer : MonoBehaviour {

	[SerializeField]
	private GameObject healthPickUp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// take Damage function for non supplied damage amount... no arguments
	public void takeDamage() 
	{
		// spawn a health beer
		GameObject health = Instantiate(healthPickUp, this.transform.position, this.transform.rotation) as GameObject;

		// Destroy this box
		Destroy(this.gameObject);
	}

	// take damage for a supplied damage amount, which will probably always be 0
	public void takeDamage(float damage) 
	{
		// spawn a health beer
		GameObject health = Instantiate(healthPickUp, this.transform.position, this.transform.rotation) as GameObject;
		
		// Destroy this box
		Destroy(this.gameObject);
	}
}
