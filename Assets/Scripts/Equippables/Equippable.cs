using UnityEngine;
using System.Collections;



// ************************************
// 		
//  Equippable - Guns or Gadgets,
//
//  
public class Equippable : MonoBehaviour {
	// Required variables
	[SerializeField]
	public Material m_material;
	[SerializeField]
	public Mesh m_mesh;
	[SerializeField]
	public Vector3 m_rotation;
	[SerializeField]
	public int ammoCount = 100;
	public int maxAmmo = 100;
	public float reloadTime;
	public float reloadTimer = 0;
	private Transform m_transform;
	public string name;
	public float damage;

	//************************************
	//
	// 		Getters and Setters
	//
	//*************************************
//	private void equip() { isEquipped = true;	}
//	private void unequipt()	{ isEquipped = false;	}
	public void setName(string n) {  this.name = n;  }
	public string getName() { return this.name; }
	private void setMaterial(Material mat) { this.m_material = mat;	}
	public Material getMaterial() {	return m_material;	}
	private void setMesh(Mesh that) { this.m_mesh = that;	}
	public Mesh getMesh() {		return m_mesh;	}
	private void setRotation(Vector3 eulerAngles) {	m_rotation = eulerAngles;	}
	public virtual Vector3 getRotation() {	return m_rotation;	}
	public void setReloadTime(float rTime) {	reloadTime = rTime;	}
	public virtual float getReloadTime() {	return reloadTime;	}	
	public void setReloadTimer(float rTime) {reloadTimer= rTime;	}
	public virtual float getReloadTimer() {	return reloadTimer;	}
	public void setTransform(Transform t) {	m_transform = t; }
	public Transform getTransform()	{ return m_transform;	}
	public virtual void setShotPower(int power)	{ return;	}
	public virtual int voidGetShotPower() { return -1;	}
	public virtual void setShotOffset(float offset)	{ return;	}
	public virtual float getShotOffset(){ return -1.0f;	}
	public virtual int getAmmo() { return this.ammoCount; }
	public virtual void setAmmo(int a) { this.ammoCount = a; }
	public virtual int getMaxAmmo() { return maxAmmo; }
	public virtual void setMaxAmmo(int m) { maxAmmo = m; }
	public virtual float getDamage() {	return this.damage; }	
	public virtual void setDamage(float d) { this.damage = d; }
	//************************************
	//
	//		Methods, functionality
	//
	//************************************
	//	 			fire() gets called as long as Fire button is down
	//					-	returns true if the weapon was able to fire, Otherwise the calling scripts updates reloadTime
	public virtual bool fireButtonDown(){	return true;	}
	public virtual void fireButtonReleased(){	return;	}
	public virtual void incReloadTimer() {	reloadTimer += Time.deltaTime;	}
	public virtual void setUpForPlay(){	  return;	}

} // End class Equiptable







