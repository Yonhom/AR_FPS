using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerWeapon))]
public class WeaponManager : NetworkBehaviour {

	private const string WEAPON_LAYER_MASK_NAME = "Weapon";

	// remote weapon Camera to disable
	[SerializeField]
	private Camera weaponCamera;

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
		CurrentWeaponSetupWithIndex (0);
		
	}

	/** for outer class to swap weapon */
	public void CurrentWeaponSetupWithIndex(int index) {
		GameObject weapon = InstantiateWeapon (weaponPrefabs [index], weaponHolder.transform);

		SetCurrentWeapon (weapon, index);

		if (isLocalPlayer) { // local player's weapon uses 'Weapon' mask, remote player's stays 'Default'
			weapon.layer = LayerMask.NameToLayer (WEAPON_LAYER_MASK_NAME);
		}

		if (weaponCamera != null) {
			if (!isLocalPlayer) { // local player's weapon uses 'Weapon' mask, remote player's stays 'Default'
				weaponCamera.enabled = false;
			}
		}
	}

	/** for outer class to retrive current weapon info */
	public PlayerWeapon GetCurrentWeaponInfo() {
		return currentWeaponInfo;
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
