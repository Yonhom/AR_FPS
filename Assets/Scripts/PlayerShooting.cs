
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShooting : NetworkBehaviour {

	// the fire starting position
	[SerializeField]
	private Camera shootingPoint;

	// mask for the target to shot. e.g. remote player
	[SerializeField]
	private LayerMask mask;

	// weapon manager for swaping weapon and getting the correspondent weapon info
	private WeaponManager weaponManager;

	void Start() {
		if (shootingPoint == null) {
			Debug.Log ("No Shooting point designated, shooting disabled!");
			this.enabled = false;
		}

		// weapon manager for changing the current weapon
		weaponManager = GetComponent<WeaponManager> ();
	}


	void FixedUpdate() {

		// remote player dont update
		if (!isLocalPlayer)
			return;

		if (weaponManager.GetCurrentWeaponInfo ().fireRate <= 0) { // not a automatic gun
			if (Input.GetButtonDown ("Fire1")) { // the left mouse button clicked
				Shoot ();
			}
		} else { // is a automatic gun
			if (Input.GetButtonDown("Fire1")) {
				InvokeRepeating ("Shoot", 0f, 1f / weaponManager.GetCurrentWeaponInfo ().fireRate);
			} 
			if (Input.GetButtonUp ("Fire1")) {
				CancelInvoke ("Shoot");
			}
		}

			
	}

	[Client] // identify that this method is only run on client side
	void Shoot() {
		Debug.Log ("Fire!");

		// store the hitting info when a collider is hit
		RaycastHit _hit;

		// start the shooting from starting point forward, only shot object marked by the layermask
		if (Physics.Raycast (shootingPoint.transform.position, 
			shootingPoint.transform.forward, out _hit, weaponManager.GetCurrentWeaponInfo().range, mask)) {
			CmdPlayerBeenShot (_hit.collider.name);
		}

	}

	 // method marked with this attribute. when this method is called, client will send a commend to server, 
		//server will call this method
	[Command]  // client --> server
	void CmdPlayerBeenShot(string _playerID) {
		// update the player's info who's being shot
		Player _playerBeingShot = GameManager.getPlayer(_playerID);
		_playerBeingShot.RpcTakeShot (weaponManager.GetCurrentWeaponInfo().damage);
	}


}
