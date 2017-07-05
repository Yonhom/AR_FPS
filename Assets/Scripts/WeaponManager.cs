using UnityEngine;
using UnityEngine.Networking;

/** weapon collections and interface for get every info about the current weapon */
[RequireComponent(typeof(PlayerWeapon))]
public class WeaponManager : NetworkBehaviour {

	private const string WEAPON_LAYER_MASK_NAME = "Weapon";

	// empty object for holding the weapon graphics
	[SerializeField]
	private GameObject weaponHolder;


	// weapons info in the weapon manager
	[SerializeField]
	private PlayerWeapon[] weapons;
	// the correspondent weapon prefab
	[SerializeField]
	private GameObject[] weaponPrefabs;

	// for holding the current weapon prefab instance
	private GameObject currentWeapon;
	// for holding the substituted weapon instance for destruction
	private GameObject previousWeapon;
	// for hold the current weapon's info
	private PlayerWeapon currentWeaponInfo;

	// Use this for initialization
	void Start () {

		// set the first weapon in the weapon manager as the initial weapon
		SetupCurrentWeaponWithIndex (0);
		
	}

	/** for outer class to swap weapon */
	public void SetupCurrentWeaponWithIndex(int index) {
		GameObject weapon = InstantiateWeapon (weaponPrefabs [index], weaponHolder.transform);

		SetCurrentWeapon (weapon, index);

		if (isLocalPlayer) { // local player's weapon and its children uses 'Weapon' mask, remote player's stays 'Default'
			Utils.AssignLayerMaskToObjectAndChildren (weapon, 
				LayerMask.NameToLayer (WEAPON_LAYER_MASK_NAME));
		}
			
	}

	/** for outer class to retrive current weapon info */
	public PlayerWeapon GetCurrentWeaponInfo() {
		return currentWeaponInfo;
	}

	/** for outer class to retrive the current weapon instance, to add muzzle flash while shooting */
	public GameObject GetCurrentWeapon() {
		return currentWeapon;
	}

	// prefab to gameobject instance
	private GameObject InstantiateWeapon(GameObject weaponPrefab, Transform parent) {
		return Instantiate (weaponPrefab, parent);
	}

	// set current weapon and previous weaponn and current weapon info
	private void SetCurrentWeapon(GameObject weapon, int weaponIndex) {
		// the the previous weapon the destruction
		if (currentWeapon != null)
			previousWeapon = currentWeapon;
		
		// set the current weapon
		currentWeapon = weapon;
		// set the current weapon info
		currentWeaponInfo = weapons[weaponIndex];

		// destroy the previsous weapon, if exists
		if (previousWeapon != null) {
			Destroy (previousWeapon);
			previousWeapon = null;
		}

	}
		

}
