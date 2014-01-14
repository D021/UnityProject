using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour {

	// Debug object
	[SerializeField]
	GameObject debugSphere;
	private bool hasHit = false;

	// This should match the radius debug sphere used with this 
	[SerializeField] 
	private float blastRadius = 2.5f;
	public float damage = 25f;

	private Transform grenadeLauncherTransform;
	// The array of object caught in this grenade blast radius once it explodes
	private	Collider[] hitColliders;

	[SerializeField]
	private AudioClip explosionClip;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision)
	{
		// if we hit the player or the players weapon, do nothing
		// This is because the grenade spawns inside the gun so the initial collision is with the weapon
		if (collision.gameObject.tag.Equals("PlayerWeapon") || collision.gameObject.tag.Equals("Player"))
		{
			// Do nothing
			return;
		}
		// First make sure we have not registered a hit on anything
		if (!hasHit)
		{
			AudioSource.PlayClipAtPoint(explosionClip, this.transform.position);
			hasHit = true;
			// Create the explosion effect
			transform.parent = transform;
			GameObject clone = Instantiate (debugSphere, this.transform.position, this.transform.rotation) as GameObject;

			// cast a sphere and do damage to breakable objects and enemies in the spheres radius
			hitColliders = Physics.OverlapSphere (this.transform.position, blastRadius);
			for (int i = 0; i < hitColliders.Length; i++)
			{
				if ( hitColliders[i].tag.Equals("Enemy") )
				{
					// We hit an enemy, call the enemies damage script
					EnemyDroneAi script = (EnemyDroneAi) hitColliders[i].GetComponent(typeof(EnemyDroneAi));
					script.takeDamage(damage);
				}
				else if ( hitColliders[i].tag.Equals("Robot") )
				{
					// We hit an enemy, call the enemies damage script
					EnemyRobotAi script = (EnemyRobotAi) hitColliders[i].GetComponent(typeof(EnemyRobotAi));
					script.takeDamage(damage);
				}

				else if ( hitColliders[i].tag.Equals("Container") )
				{
					HealthContainer script = (HealthContainer) hitColliders[i].GetComponent(typeof(HealthContainer));
					script.takeDamage();
				}
			}
			// Destroy the grenade (it blows up)
			Destroy(this.gameObject);
		}
	}
}
