using UnityEngine;
using System.Collections;

public class LaserGun : Equippable {
	// For rigidBody Ammo
	[SerializeField]
	private int shotPower;
	[SerializeField]
	private float shotOffset;
	[SerializeField]
	private int ammo = 100;
	[SerializeField] 
	private int shotDistance = 20;
	private int m_reloadTime;
	private Vector3 laser_rotation = new Vector3(270, 270, 0);
	private bool instantiateLaser = true;
	[SerializeField]
	private LineRenderer lineRenderer;
	RaycastHit hitInfo;
	private float laserDistance = 80f;
	AudioSource shotSoundClip;



	public override void setUpForPlay ()
	{
		this.name = "Laser";

		lineRenderer = Instantiate(Resources.Load("Weapons/LaserGun/LaserRenderer", typeof(LineRenderer))) as LineRenderer;
		lineRenderer.transform.position = new Vector3(0,0,0);
		lineRenderer.transform.eulerAngles = new Vector3(0,0,270);
		lineRenderer.useWorldSpace = true;

		// Load SoundClip
		shotSoundClip = Instantiate(Resources.Load("Sounds/Weapons/LaserGun/LaserAudioSource", typeof(AudioSource))) as AudioSource;
		shotSoundClip.enabled = true;
		shotSoundClip.transform.parent = this.transform.parent;
	}
	
	public override bool fireButtonDown()	{
		float tempLaserDistance = 80f;
		// Draw a line out from the barrel for debugging
		Debug.DrawRay(this.getTransform().position, this.getTransform().right*10, Color.white);
		// cast out a ray to detect what we are hitting
		Ray ray = new Ray(this.getTransform().position, this.getTransform().right + Vector3.one);
		if (Physics.Raycast(this.getTransform().position, this.getTransform().right, out hitInfo, tempLaserDistance) )
		{
			Debug.Log("HitSomething");
			// Now that we have hit somthing, change the laserDistance
			tempLaserDistance = (hitInfo.transform.position - this.getTransform().position).magnitude;
			Debug.Log(tempLaserDistance);
			if (hitInfo.transform.tag.Equals("EnemyDrone"))
			{
				EnemyDroneAi script = (EnemyDroneAi) hitInfo.transform.GetComponent(typeof(EnemyDroneAi));
				script.takeDamage();
			}
			else if ( hitInfo.transform.tag.Equals("Enemy") )
			{
				// We hit an enemy, call the enemies damage script
				EnemyAi script = (EnemyAi) hitInfo.transform.GetComponent(typeof(EnemyAi));
				script.takeDamage(3f);
			}
		}

		// Draw our lineRenderer
		lineRenderer.SetPosition(0, this.getTransform().position);
		lineRenderer.SetPosition(1, this.getTransform().position + this.getTransform().right*tempLaserDistance);
		Debug.DrawRay(this.getTransform().position, this.getTransform().right * tempLaserDistance, Color.white);

		// Play sound
		shotSoundClip.Play();
		return false;
	}

	public override void fireButtonReleased()	{
		lineRenderer.SetPosition(0, this.getTransform().position);
		lineRenderer.SetPosition(1, this.getTransform().position);

		shotSoundClip.Stop();
	}
		
	// ReloadTime
	public override float getReloadTime() {	return m_reloadTime;}
	// Shot Power
	public override void setShotPower(int power) { shotPower = power;  }
	public override int voidGetShotPower(){	return shotPower;	}
	// Shot Offset
	public void setShotOffset(float offset)	{shotOffset = offset;}
	public float getShotOffset(){ return shotOffset;	}
	// rotation
	public override Vector3 getRotation() {	return laser_rotation;	}

}

