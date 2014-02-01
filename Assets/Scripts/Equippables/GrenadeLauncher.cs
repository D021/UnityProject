using UnityEngine;
using System.Collections;

public class GrenadeLauncher : Equippable {
	[SerializeField]
	private float shotOffset;
	[SerializeField]
	private Grenade grenade;
	[SerializeField]
	public int shotPower = 1000;
	private float m_reloadTime = 0.5f;
	[SerializeField]
	private AudioSource shotSoundClip;

	public override void setUpForPlay()
	{
		this.transform.position = new Vector3(0.01f, -0.12f, 0f);
		this.transform.eulerAngles = new Vector3(0f, 180f, 90f);

		shotSoundClip = Instantiate(Resources.Load("Sounds/Weapons/GrenadeLauncher/GrenadeLauncherAudioSource", typeof(AudioSource))) as AudioSource;
		shotSoundClip.enabled = true;
		shotSoundClip.transform.parent = this.getTransform();

		this.name = "Grenade Launcher";
		this.damage = 25f;
		this.ammoCount = 100;
		this.maxAmmo = 100;

	}
	
	public override bool fireButtonDown()
	{
		// We can fire the launcher
		if (this.reloadTimer > this.m_reloadTime && (ammoCount > 0) ) // hard code for now, should be this, but this is not set
		{
			// DEBUG
			Debug.Log("FireButtonGL");

			// Play shot sound
			shotSoundClip.Play();

			// Create a new bullet
			Grenade clone = Instantiate (grenade, this.getTransform().position + this.getTransform().forward * shotOffset, this.transform.rotation) as Grenade;
			clone.damage = damage;
			clone.rigidbody.AddForce(this.getTransform().forward * shotPower);

			// Reset reload time
			this.reloadTimer = 0f;	// Begin reloading

			// decrement ammo count
			ammoCount--;
			return true;
		}
		return false;
	}

	/* gets and sets */

	// RELOAD TIME
	public override float getReloadTime() {	return m_reloadTime; }	
	public void setReloadTime(float r) { reloadTime = 0; }
	// SHOW POWER
	public override void setShotPower(int power) { this.shotPower = power;}
	public override int voidGetShotPower() { return this.shotPower;	}
	// DAMAGE
	public override float getDamage() {	return this.damage; }	
	public override void setDamage(float d) { this.damage = d; }


	public void setShotOffset(float offset)	{ this.shotOffset = offset; }
	public float getShotOffset() { return this.shotOffset; }

}
