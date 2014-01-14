using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {
	[SerializeField]
	private float spawnTime;
	[SerializeField]
	GameObject enemyToSpawn;
	[SerializeField]
	private float spawnTimer = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer += Time.deltaTime;
		if (spawnTimer > spawnTime)
		{
			spawnTimer = 0;
			Instantiate(Resources.Load("Enemies/Robot", typeof(GameObject)), this.transform.position, this.transform.rotation);
		}
	}
}
