using UnityEngine;
using System.Collections;

public class GrenadeLauncher : Equippable {
	// For rigidBody Ammo
	[SerializeField]
	private float shotOffset;
	[SerializeField]
	private int ammo = 100;
	[SerializeField]
	private GameObject grenade;
	public int shotPower = 1000;
	private int m_reloadTime = 2;

	void Start()
	{

	}

	public override bool fireButtonDown()
	{
		// We can fire the launcher
		if (this.reloadTimer > this.m_reloadTime) // hard code for now, should be this, but this is not set
		{
			// Create a new bullet
			Debug.Log("FireButtonGL");
			GameObject clone = Instantiate (grenade, this.getTransform().position + this.getTransform().forward * shotOffset, this.transform.rotation) as GameObject;
			// Fire bullet directly forward
			clone.rigidbody.AddForce(this.getTransform().forward * shotPower);
			// Reset reload time
			this.reloadTimer = 0f;	// Begin reloading
			return true;
		}
		return false;
	}

	public override float getReloadTime() {
		return m_reloadTime;
	}
	
	public override void setShotPower(int power)
	{
		this.shotPower = power;
	}
	public override int voidGetShotPower()
	{
		return this.shotPower;
	}
	public void setShotOffset(float offset)
	{
		this.shotOffset = offset;
	}
	public float getShotOffset()
	{
		return this.shotOffset;
	}
}
