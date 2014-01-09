using UnityEngine;
using System.Collections;



// ************************************
// 		
//  Equiptable - Guns or Gadgets,
//
//  Must provide:
//
//		Variables:
//			Mesh				
//			Material
//			Rotation - to correctly orient on the hand
//
//		Functions:
//			fire()
//
//
//	 			fire() gets called whenever user presses the Fire button on the keypad or controller
//					-	returns true if the weapon was able to fire, Otherwise the calling scripts updates reloadTime
//	
//				incReloadTimer() called once by the calling script as soon as Equiptable is switched
//					-	returns timeToReload;
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
	public int ammoCount;
	public float reloadTime;
	public float reloadTimer = 0;

	// Use this for initialization
	private bool isEquipped;

	private Transform m_transform;
	//************************************
	//
	// 		Getters and Setters
	//
	//*************************************
	private void equip()
	{
		isEquipped = true;
	}
	private void unequipt()
	{
		isEquipped = false;
	}
	private void setMaterial(Material mat) {
		this.m_material = mat;
	}
	public Material getMaterial() {
		return m_material;
	}
	private void setMesh(Mesh that) {
		this.m_mesh = that;
	}
	public Mesh getMesh() {
		return m_mesh;
	}
	private void setRotation(Vector3 eulerAngles) {
		m_rotation = eulerAngles;
	}
	public Vector3 getRotation() {
		return m_rotation;
	}
	public void setReloadTime(float rTime) {
		reloadTime = rTime;
	}
	public virtual float getReloadTime() {
		return reloadTime;
	}
	public void setReloadTimer(float rTime) {
		reloadTimer= rTime;
	}
	public virtual float getReloadTimer() {
		return reloadTimer;
	}
	public void setTransform(Transform t) {
		m_transform = t;
	}
	public Transform getTransform()
	{
		return m_transform;
	}
	public virtual void setShotPower(int power)
	{
		return;
	}
	public virtual int voidGetShotPower()
	{
		return -1;
	}
	public virtual void setShotOffset(float offset)
	{
		return;
	}
	public virtual float getShotOffset()
	{
		return -1.0f;
	}
	
	//************************************
	//
	//		Methods, functionality
	//
	//************************************
	//	 			fire() gets called as long as Fire button is down
	//					-	returns true if the weapon was able to fire, Otherwise the calling scripts updates reloadTime
	public virtual bool fireButtonDown()	{
		return true;
	}

	public virtual void incReloadTimer()	{
		reloadTimer += Time.deltaTime;
	}

} // End class Equiptable







