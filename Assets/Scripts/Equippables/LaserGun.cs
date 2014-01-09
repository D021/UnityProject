using UnityEngine;
using System.Collections;

public class Laser : Equippable {
	// For rigidBody Ammo
	[SerializeField]
	private int shotPower;
	[SerializeField]
	private float shotOffset;
	[SerializeField]
	private int ammo = 100;
	private int m_reloadTime;
	
	public Laser()
	{
		string pathToMat = PlayerPrefs.GetString("LaserMaterial");
		string pathToMesh = PlayerPrefs.GetString("LaserMesh");
	}
	public Laser(int ammo)
	{

	}
	
	public override bool fireButtonDown()	{
		// Draw a line out from the barrel for debugging
		Debug.DrawRay(this.transform.position, this.transform.right*10, Color.red);
		// cast out a ray to detect what we are hitting

		Ray ray = new Ray(this.transform.position, this.transform.right*10);
		RaycastHit hitInfo;
			
		if(	Physics.Raycast(ray, out hitInfo, 10) )
		{
			if (hitInfo.transform.tag.Equals("Enemy"))
			{
				EnemyAi script = (EnemyAi) hitInfo.transform.GetComponent(typeof(EnemyAi));
				script.takeDamage();
			}
			else if ( hitInfo.transform.tag.Equals("Robot") )
			{
				// We hit an enemy, call the enemies damage script
				EnemyRobotAi script = (EnemyRobotAi) hitInfo.transform.GetComponent(typeof(EnemyRobotAi));
				script.takeDamage(3f);
			}
		}
		return false;
	}
		
	// ReloadTime
	public override float getReloadTime() {	return m_reloadTime;}
	// Shot Power
	public override void setShotPower(int power) { shotPower = power;  }
	public override int voidGetShotPower(){	return shotPower;	}
	// Shot Offset
	public void setShotOffset(float offset)	{shotOffset = offset;}
	public float getShotOffset(){ return shotOffset;	}
}

