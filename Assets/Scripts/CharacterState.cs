using UnityEngine;
using System.Collections;


// This class managers the state that the player is in
// Things such as health, armour, any active powerups, experience...etc
//
public class CharacterState : MonoBehaviour {

	// private variables
	[SerializeField]
	private int health;
	[SerializeField]
	private GUIText guiText;
	[SerializeField]
	private Transform activeWeapon;

	// Use this for initialization
	void Start () {
		health = 3;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	// Similar to Update, called every frame
	void OnGUI () {
		GUI.Box (new Rect (0,0,100,50), "Health\n" + health.ToString());

		// Shit, we died
		if (health <= 0) 
		{
		 	int w = 250;
			int h = 50;
			Rect rect = new Rect((Screen.width-w)/2, (Screen.height-h)/2, w, h);
			GUI.Box(rect, "You Died\n Health <= 0");

		}
	}



	// Public Methods, can be called by other Game Objects (enemies, health pick ups, projectiles, ...etc)

	// A general take damage Functions
	// no arguments
	public void takeDamage() 
	{

	}

	// A more specific take damage functions
	// argument is amount of health to take off
	// @param damageAmount amount of damage to deduct
	public void takeDamage (int damageAmount) 
	{
		// Deduct the amount of health
		health -= damageAmount;
		// Did we die?
		if (health <= 0)
		{
			// Yep

		}
		else
		{
			// Nope, still alive

		}
	}

	public void incrementHealth(int amount)
	{
		health += amount;
	}


}
