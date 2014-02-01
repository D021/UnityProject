using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WeaponHand : MonoBehaviour {

	//[SerializeField]
	//private GameObject laserPrefab;

	//
	[SerializeField]
	private bool fired;
	private bool reload;
	private bool rightBumperDown;
	[SerializeField]
	private float reloadTime;
	[SerializeField]
	private float reloadTimer;

	// Info for different weapon types
	private bool meleeWeapon;
	public int weaponNum = 0;
	[SerializeField]

	// This enum should match arrays above
	public enum WeaponType
	{
		Launcher,			// 0
		Laser,				// 1
	}


	// Input variables
	private float inputFire;
	private bool inputChangeWeapon;

	private GameObject go;
	private WeaponType weaponType = WeaponType.Launcher;

	// Player info
	private GameObject player;
	private GameObject rightHand;

	// *****************************************************
	// Array of equiptable items such as Gadgets and Weapons
	// *****************************************************
	Equippable currentEquippedWeapon;
	[SerializeField]
	private List<Equippable> equips = new List<Equippable>();
	private int i = 0;


	private MeshFilter m_meshFilter;
	private MeshRenderer m_MeshRendered;
	/// <summary>
	private bool canFire;

	/// 
	/// 
	/// 
	// Use this for initialization
	void Start () {

		WeaponManager wm = this.GetComponent<WeaponManager>();
		Debug.Log("WeaponHand");
		wm.setUpForPlay();
		wm.loadUnlockedWeapons(ref equips);


		currentEquippedWeapon = equips[0];
		fired = false;

		player = GameObject.FindWithTag("Player");
		rightHand = GameObject.Find("Beta/Beta:Hips/Beta:Spine/Beta:Spine1/Beta:Spine2/Beta:RightShoulder/Beta:RightArm/Beta:RightForeArm/Beta:RightHand");



		// Draw the initial weapons mat and mes
		renderer.material = currentEquippedWeapon.getMaterial();
		this.GetComponent<MeshFilter>().mesh  = currentEquippedWeapon.getMesh();
		this.transform.localEulerAngles = currentEquippedWeapon.getRotation();
		currentEquippedWeapon.setTransform(this.transform);
		// Load List of available weapons from InventoryManager

		//Debug.Log(reloadTime);
		// For rigidBody Ammo
		// Default is grenade launcher, this code will be changed once we have a filesystem or database to load from 
		reloadTime = currentEquippedWeapon.getReloadTime();
		currentEquippedWeapon.setReloadTimer(reloadTime+1.0f);
		reloadTimer = currentEquippedWeapon.getReloadTimer();
		currentEquippedWeapon.setReloadTime(0.5f);
		//equips[0].setShotPower(1000);
		currentEquippedWeapon.setShotOffset(2);
		currentEquippedWeapon.setShotPower(900);



		/*************************************/
		// Add Weapons to equips
		// equips[i] = new Laser();
	}
	
	// Update is called once per frame
	void Update () 
	{

		// If Fire Button / Right Trigger is Down
		if (Input.GetAxis("Fire") > 0.1f)
		{	
			fired = true;
			currentEquippedWeapon.fireButtonDown();
		}
		else if (fired)
		{
			fired = false;
			currentEquippedWeapon.fireButtonReleased();
		}
		// if Switch Weapon Button is down / Right Bumper
		else if (Input.GetButtonDown("RightBumper") )
		{
			Debug.Log("SwitchWeapon");
			switchWeapon(); // or gadget or whatever
		}
		currentEquippedWeapon.incReloadTimer();
	}

	//
	void switchWeapon()
	{
		if (++i >= equips.Count)
		{
			i = 0;
		}

		// DEBUG
		Debug.Log(i);
		Debug.Log(equips[i].getMaterial());
		Debug.Log(equips[i].getMesh());
		Debug.Log(equips[i].getRotation());


		// Switch Weapon type
		weaponType = (WeaponType)weaponNum;

		// Change mesh and material
		renderer.material = equips[i].getMaterial();
		this.GetComponent<MeshFilter>().mesh  = equips[i].getMesh();
		// correctly orient the weapon
		this.transform.localEulerAngles = equips[i].getRotation();
		currentEquippedWeapon = equips[i];
		currentEquippedWeapon.setTransform(this.transform);
	}

	/*****************************************/
	/************					**********/
	/************	Fire Modes		**********/
	//  
	// Fires a laser using a ray cast*/
	void fireLaser()
	{
		// Draw a line out from the barrel for debugging
		Debug.DrawRay(this.transform.position, this.transform.right*10, Color.red);
		// cast out a ray to detect what we are hitting
		
		Ray ray = new Ray(this.transform.position, this.transform.right*10);
		RaycastHit hitInfo;
			
		if(	Physics.Raycast(ray, out hitInfo, 10) )
		{
			if (hitInfo.collider.gameObject.tag == "Enemy") {
				go = hitInfo.collider.gameObject;
				EnemyAi script = (EnemyAi) go.GetComponent(typeof(EnemyAi));
				script.takeDamage();
				//Destroy(hitInfo.collider.gameObject);
			}
			if ( hitInfo.transform.tag.Equals("Robot") )
			{
				// We hit an enemy, call the enemies damage script
				EnemyRobotAi script = (EnemyRobotAi) hitInfo.transform.GetComponent(typeof(EnemyRobotAi));
				script.takeDamage(3f);
			}
		}
	}
}
