using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	//[SerializeField]
	//private GameObject laserPrefab;

	//public LineRenderer laser;
	[SerializeField]
	private bool fired;
	private bool reload;
	private bool rightBumperDown;
	[SerializeField]
	private float reloadTime;
	[SerializeField]
	private float reloadTimer;
	[SerializeField]
	private float shotPower;
	[SerializeField]
	private float shotOffset;
	
	// Info for different weapon types

	private bool meleeWeapon;
	public int weaponNum = 0;
	[SerializeField]
	private Material[] materials;
	[SerializeField]
	private Mesh[] meshes;
	[SerializeField]
	private Vector3[] rotations;
	// This enum should match arrays above
	public enum WeaponType
	{
		Launcher,			// 0
		Laser,				// 1
	}

	// Ammo or bullets or whatever, these are the gameobject that will be fired/activated from the weapon
	[SerializeField]
	private GameObject grenade;


	// Input variables
	private float inputFire;
	private bool inputChangeWeapon;

	private GameObject go;
	private WeaponType weaponType = WeaponType.Launcher;

	// Player info
	private GameObject player;
	private GameObject rightHand;
	// Use this for initialization
	void Start () {
		fired = false;

		player = GameObject.FindWithTag("Player");
		rightHand = GameObject.Find("Beta/Beta:Hips/Beta:Spine/Beta:Spine1/Beta:Spine2/Beta:RightShoulder/Beta:RightArm/Beta:RightForeArm/Beta:RightHand");

		meleeWeapon = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		inputFire = Input.GetAxis("Fire");
		inputChangeWeapon = Input.GetButton("RightBumper");
	
		// Change weapon
		if (inputChangeWeapon && !rightBumperDown)
		{
			rightBumperDown = true;
			switchWeapon();
		}
		if (!inputChangeWeapon)
		{
			rightBumperDown = false;
		}

		// Draw reticle/targetspot/aim position, whatever you'd like to call it

		// Check if right Trigger is pressed
		if (inputFire > 0.1f)
		{
			fired = true;
			switch (weaponType) 
			{
			case WeaponType.Laser :
				fireLaser();
				reload = false;
				break;
			case WeaponType.Launcher :
				fireGrenade();
				reload = true;
				break;
			}
		}

		// Adjust reload timer if weapon is reloadable
		if (reload)
		{
			reloadTimer += Time.deltaTime;
		}
	}

	//
	void switchWeapon()
	{
		if (++weaponNum >= meshes.Length)
		{
			weaponNum = 0;
		}
		// Switch Weapon type
		weaponType = (WeaponType)weaponNum;

		// Change mesh and material
		renderer.material = materials[(int)weaponType];
		this.GetComponent<MeshFilter>().mesh = meshes[(int)weaponType];

		// correctly orient the weapon
		this.transform.localEulerAngles = rotations[(int)weaponType];

	}

	// Fires a laser using a ray cast
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

	// Fires a grenade using a prefab and projectile
	void fireGrenade()
	{
		if (reloadTimer > reloadTime)
		{
			// Create a new bullet
			GameObject clone = Instantiate (grenade, this.transform.position + this.transform.forward * shotOffset, this.transform.rotation) as GameObject;
			// Fire bullet directly forward
			clone.rigidbody.AddForce(this.transform.forward * shotPower);
			// Reset reload time
			reloadTimer = 0f;	// Begin reloading
		}
	}
}
