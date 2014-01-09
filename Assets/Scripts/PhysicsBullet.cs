using UnityEngine;
using System.Collections;

public class ExplodingTechBullet : MonoBehaviour {

	[SerializeField]
	private float lifeSpan = 5f;
	[SerializeField] 
	private float lifeSpanTimer;

	// The transform that this projectile hits, gets set on OnCollisionEnter
	private Transform collidedTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		lifeSpanTimer += Time.deltaTime;
		if (lifeSpanTimer >= lifeSpan) 
		{
			Destroy(this);
		}
	}

	void OnCollisionEnter(Collision collision) 
	{
		ContactPoint contact = collision.contacts[0];
		// We hit the Player
		if (collision.gameObject.tag.Equals("Player") )
		{
			Debug.Log("Player was hit");
			// Get the transform first
			collidedTransform = collision.gameObject.transform;
			// get character state script
			CharacterState script = (CharacterState) collidedTransform.GetComponent(typeof(CharacterState));
			script.takeDamage(1);
		}
	}
}
