
using UnityEngine;

[System.Serializable] // wherever this class is referenced, its properties is configurable in that object that refers it
public class PlayerWeapon {

	public string weaponName;

	public int damage;

	public float range;

	/** 0: one tap, one shot; >0, automatic */
	public int fireRate;

}
