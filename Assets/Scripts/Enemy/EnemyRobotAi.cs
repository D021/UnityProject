using UnityEngine;
using System.Collections;

public class EnemyRobotAi : MonoBehaviour {

	// Navigation, visual and sound awareness
	private GameObject player;	// The player
	private Transform playerTransform;
	private Transform enemyTransform;
	private bool detectsPlayer = true;
	private Vector3 toPlayer;


	// This enemies Components
	private CharacterController enemyCharacterController;
	private Animator animator;

	[SerializeField]
	private float health = 100f;
	[SerializeField]
	private float speed = 0.3f;
	[SerializeField]
	private float shotDistance = 5f;
	[SerializeField]
	private float meleeDistance = 2f;
	[SerializeField]
	private bool dying = false;
	[SerializeField]
	private float deathTime;
	[SerializeField]
	private float deathTimer = 0f;

	private Transform healthTextTransform;
	private TextMesh healthText;
	private Vector3 myLookAt;
	// Use this for initialization
	void Start () {
		enemyCharacterController = this.GetComponent<CharacterController>();
		animator = this.GetComponent<Animator>();

		player = GameObject.FindGameObjectWithTag("Player");
		playerTransform = player.transform;

		healthTextTransform = transform.FindChild("EnemyText");
		healthText = healthTextTransform.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
		healthText.text = health.ToString("N2");
	}
	
	// Update is called once per frame
	void Update () {
		// DEBUG
		healthText.text = health.ToString("N2");

		// the enemy is dying so check if dying animation is finished, if so, destroy gameobject, otherwise return
		if (dying) 
		{
			//animator.SetBool("Die", false);
			deathTimer += Time.deltaTime;
			if(deathTimer > deathTime)
			{
				Destroy(this.gameObject);
			}

		}
		else
		{
			toPlayer = playerTransform.position - this.transform.position;
			if (detectsPlayer)
			{
				myLookAt = playerTransform.position;
				myLookAt.y = 0;
				this.transform.LookAt(myLookAt);
				// Enemy is engaging the player
				// Draw a line to player DEBUGGING
				// Debug.DrawLine(this.transform.position, playerTransform.position);

				//  if close enough to strike
				if (toPlayer.magnitude < meleeDistance)
				{

					meleeAttack();
				}
				else
				{
					// Close enough to player to do a melee attack
					goToPlayer();
				}

			}
		}
	}

	// move to player
	private void goToPlayer()
	{
		toPlayer = playerTransform.position - this.transform.position;
		toPlayer.y = 0;
		enemyCharacterController.Move((toPlayer.normalized) * Time.deltaTime * speed);
		animator.SetFloat("Speed", 1.0f);
	}

	private void meleeAttack()
	{
		//animator.g
	}

	// script accessicble by other gameobjects to make this guy take some damage
	public void takeDamage(float amount)
	{
		health -= amount;
		if (health <= 0 && !dying)
		{
			dying = true;
			animator.SetBool("Die", true);
		}
	}
	
}
