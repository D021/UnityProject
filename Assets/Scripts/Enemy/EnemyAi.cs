using UnityEngine;
using System.Collections;

public class EnemyAi : MonoBehaviour {

	private float health = 100;

	private GameObject player;
	private Transform enemyTransform;
	private Transform playerTransform;	// The players feet
	private Vector3 toPlayer;

	private Transform healthTextTransform;
	private TextMesh healthText;
	// Use this for initialization
//	void Start () 
//	{
//		if (speed == 0) {
//			speed = 0.1f;
//		}
//		enemyTransform = this.transform;
//		player = GameObject.FindGameObjectWithTag("PlayerTransform");
//		playerTransform = player.transform;
//		enemyCharacterController = this.GetComponent<CharacterController>();
//		reloadTimer = 0f;
//		reloadTime = 2f;
//
//		healthTextTransform = transform.FindChild("EnemyText");
//		healthText = healthTextTransform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
//		healthText.text = health.ToString("N2");
//	}
	
	// Update is called once per frame
//	void Update () 
//	{
//		// Debug text
//
//
//		// Update reload time
//		reloadTimer += Time.deltaTime;
//		// Debug path to player
//		Debug.DrawLine(enemyTransform.position, playerTransform.position);
//		toPlayer = playerTransform.position - enemyTransform.position;
//
//		// Enemy sees player!! Attack mode!
//		this.transform.LookAt (playerTransform);	// Vector3.up * 2
//		if (shootDistance < toPlayer.magnitude) 
//		{
//			// Path to Player
//			goToPlayer();
//		}
//		else 
//		{
//			// Shoot at player
//			fire();
//		}
//}

	public void myStart() {
		healthTextTransform = transform.FindChild("EnemyText");
		healthText = healthTextTransform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
		healthText.text = this.getHealth().ToString("N2");
	}

	void goToPlayer () 
	{

	}
	 
	void fire()
	{

	}

	public void takeDamage()
	{

	}

	public virtual void takeDamage(float damage) { 
		// DEBUG update HealthText
		healthText.text = this.getHealth().ToString("N2");
		return;	
	}

	public float getHealth() { return this.health; }

	public void setHealth(float h) { this.health = h; }

}
