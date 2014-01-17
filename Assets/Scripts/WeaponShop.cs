using UnityEngine;
using System.Collections;
using System.Collections.Generic;	// For List<>


public class WeaponShop : MonoBehaviour {
	[SerializeField]
	private TextMesh activationText;
	private bool inShop = false;
	private bool inRange = false;
	private Transform playerTrans;

	// Shop Window
	private WeaponManager wm;
	private Rect windowRect = new Rect (20, 20, 500, 500);

	private float GLdamageSlider;
	private float GLmaxAmmoSlider;
	private float GLammoCountSlider;

	private List<Equippable> unlockedWeapons;
	private List<Equippable> lockedWeapons;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (inShop) 
		{
			if (Input.GetButtonDown ("Y") )
			{
				// Make saved changes here


				inShop = false;
				Time.timeScale = 1;
			}
		}
		else if (inRange) {
			// Always face text to player
			activationText.transform.LookAt(new Vector3(playerTrans.position.x, 2.2f, playerTrans.position.z));
			activationText.transform.Rotate(new Vector3(360,180,0));

			// Check for input
			if (Input.GetButtonDown("Y") )
			{
				inShop = true;
				Time.timeScale = 0;

				wm = playerTrans.GetComponentInChildren<WeaponManager>();
				wm.loadUnlockedWeapons(ref unlockedWeapons);
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.transform.tag.Equals("Player") )
		{
			// Display text
			playerTrans = col.transform;
			activationText.text = "Activate";
			activationText.transform.LookAt(new Vector3(col.transform.position.x, 2.2f, col.transform.position.z));
			activationText.transform.Rotate(new Vector3(360,180,0));
			inRange = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.transform.tag.Equals("Player") ) 
		{
			activationText.text = "";
			inRange = false;
		}
	}

	void OnGUI() {
		if (inShop) {
			windowRect = GUI.Window(0, windowRect, WindowFunction, "Weapons Bitch");
			return;
		}
	}

	void WindowFunction (int windowID) {
		// Print out available weapons
		GUI.Label(windowRect, "UnlockedWeapons");
		int offset = 35;
		for (int i = 0; i < unlockedWeapons.Count; i++) 
		{
			GUI.Label(new Rect(25, offset, 500, 500), unlockedWeapons[i].getName() );
			offset += 15;

			if (unlockedWeapons[i].getName().Equals("Grenade Launcher") )
			{
				GUI.Label(new Rect(30, offset, 500, 500), "Damage" );
				offset += 20;
				GLdamageSlider = GUI.HorizontalSlider (new Rect (40, offset, 100, 30), GLdamageSlider, 0, 100);
				unlockedWeapons[i].setDamage(GLdamageSlider);
				offset+=15;
				GUI.Label(new Rect(30, offset, 500, 500), "Max Ammo" );
				offset += 20;
				GLmaxAmmoSlider = GUI.HorizontalSlider (new Rect (40, offset, 100, 30), GLmaxAmmoSlider, 0, 100);
				unlockedWeapons[i].setMaxAmmo(Mathf.RoundToInt(GLmaxAmmoSlider));
				offset+=15;
				GUI.Label(new Rect(30, offset, 500, 500), "Ammo" );
				offset += 20;
				GLammoCountSlider = GUI.HorizontalSlider (new Rect (40, offset, 100, 30), GLammoCountSlider, 0, 100);
				unlockedWeapons[i].setAmmo(Mathf.RoundToInt(GLammoCountSlider));
				offset+=15;

			}


		}

	}

}
