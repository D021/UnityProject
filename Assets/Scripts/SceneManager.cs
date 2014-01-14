using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	[SerializeField]
	private float spawnTime;
	[SerializeField]
	private float spawnTimer = 0f;
	[SerializeField]
	private Transform spawnLocation;
	[SerializeField]
	private GameObject enemy;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer += Time.deltaTime;
		// Spawn an enemy Droid every 10 seconds
		if (spawnTimer > spawnTime) 
		{
			// spawn enemy
			//"Object1", typeof(GameObject)) as GameObject
			enemy = Instantiate(Resources.Load("Enemies/Enemy", typeof(GameObject)),  spawnLocation.position, spawnLocation.rotation) as GameObject;
			// enemy = Instantiate(enemy, spawnLocation.position, spawnLocation.rotation) as GameObject; 
			// reset spawn timer
			spawnTimer = 0;
		}
	}
}
