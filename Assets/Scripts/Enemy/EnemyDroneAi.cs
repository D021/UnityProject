using UnityEngine;
using System.Collections;

public class EnemyDroneAi : EnemyAi {

	[SerializeField]
	private float speed;
	[SerializeField]
	private float shootDistance;
	[SerializeField]
	private float health;
	[SerializeField]
	private float reloadTime;
	[SerializeField]
	private float reloadTimer;
	[SerializeField]
	private float shotOffset;
	[SerializeField]
	private float shotPower;
	[SerializeField]
	private float maxAltitude;

	private GameObject player;
	private Transform enemyTransform;
	private Transform playerTransform;	// The players feet
	private CharacterController enemyCharacterController;
	private Vector3 toPlayer;
	public GameObject projectile;

	private Transform healthTextTransform;
	private TextMesh healthText;
	// Use this for initialization
	void Start () 
	{
		if (speed == 0) {
			speed = 0.1f;
		}
		enemyTransform = this.transform;
		player = GameObject.FindGameObjectWithTag("PlayerTransform");
		playerTransform = player.transform;
		enemyCharacterController = this.GetComponent<CharacterController>();
		reloadTimer = 0f;
		reloadTime = 2f;

		healthTextTransform = transform.FindChild("EnemyText");
		healthText = healthTextTransform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
		healthText.text = health.ToString("N2");
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Debug text


		// Update reload time
		reloadTimer += Time.deltaTime;
		// Debug path to player
		// Debug.DrawLine(enemyTransform.position, playerTransform.position);
		toPlayer = playerTransform.position - enemyTransform.position;

		// Enemy sees player!! Attack mode!
		this.transform.LookAt (playerTransform);	// Vector3.up * 2
		if (shootDistance < toPlayer.magnitude) 
		{
			// Path to Player
			goToPlayer();
		}
		else 
		{
			// Shoot at player
			fire();
		}

	}

	void goToPlayer () 
	{
		toPlayer = playerTransform.position - enemyTransform.position;
		float up = 0;


		// Check if there is an obstacle in the way
		RaycastHit hitInfo;
		if (Physics.Raycast (transform.position, toPlayer, out hitInfo, 30) )
		{
			// Detected somthing, player, ground, wall?
			// hitInfo.ToString();
			//Debug.Log(hitInfo.collider.gameObject.tag);

			// There is an obstacle in the way, increase altitude
			if (hitInfo.collider.gameObject.tag == "Obstacle") 
			{
				up += 10;
			}
		}
		enemyCharacterController.Move((toPlayer.normalized + Vector3.up * up) * Time.deltaTime * speed);
	}
	 
	void fire()
	{
		if (reloadTimer > reloadTime)
		{
			// Create a new bullet
			GameObject clone = Instantiate (projectile, this.transform.position + this.transform.forward * shotOffset, this.transform.rotation) as GameObject;
			// Fire bullet directly forward
			clone.rigidbody.AddForce(this.transform.forward * shotPower );
			// Reset reload time
			reloadTimer = 0f;	// Begin reloading
		}
	}

	public void takeDamage()
	{
		
		health-= 3.0f;
		healthText.text = health.ToString("N2");
		if (health <= 0) 
		{
			Destroy(this.gameObject);
		}
	}

	public void takeDamage(float damage)
	{
		
		health -= damage;
		healthText.text = health.ToString("N2");
		if (health <= 0) 
		{
			Destroy(this.gameObject);
		}
	}


}
