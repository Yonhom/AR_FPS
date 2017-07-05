
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

	// for spawning hit effect in the hit position
	[SerializeField]
	private GameObject hitEffectPrefeb;
	private GameObject hitEffect;

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

		// add local player's weapon with muzzle flash effect, and sync it over all clients
		CmdOnShoot();

		// store the hitting info when a collider is hit
		RaycastHit _hit;

		// start the shooting from starting point forward, only shot object marked by the layermask
		if (Physics.Raycast (shootingPoint.transform.position, 
			shootingPoint.transform.forward, out _hit, weaponManager.GetCurrentWeaponInfo().range, mask)) {

			if(_hit.collider.tag == Constants.PLAYER_TAG)
				CmdRemotePlayerBeenShot (_hit.collider.name);
			// add hit effect on everything that's been hit
			CmdOnHit (_hit.point, _hit.normal);
		}

	}

	[Command] 
	void CmdOnHit(Vector3 _pos, Vector3 _normal) {
		RpcOnHit (_pos, _normal);
	}

	[ClientRpc]
	void RpcOnHit(Vector3 _pos, Vector3 _normal) {
		hitEffect = Instantiate (hitEffectPrefeb, _pos, Quaternion.LookRotation(_normal));
		Destroy (hitEffect, 1f);
	}

	[Command] // client --> server
	void CmdOnShoot() {
		RpcOnShoot ();
	}

	// server --> clients   (has to send server param to clients)
	[ClientRpc]
	void RpcOnShoot() {
		// get the current shooting weapon
		GameObject _currentWeapon = weaponManager.GetCurrentWeapon();

		// get the player being shooting's weapon's first muzzle flash 
		ParticleSystem muzzleFlash = _currentWeapon.GetComponentInChildren<ParticleSystem>();
		if (muzzleFlash == null)
			Debug.LogError ("No muzzle flash particle system component found in: " + name);
		muzzleFlash.Play ();
	}

	 // method marked with this attribute. when this method is called, client will send a commend to server, 
		//server will call this method
	[Command]  // client --> server
	void CmdRemotePlayerBeenShot(string _playerID) {

		// update the player's info who's being shot
		Player _playerBeingShot = GameManager.getPlayer(_playerID);
		_playerBeingShot.RpcTakeShot (weaponManager.GetCurrentWeaponInfo().damage);
	}


}
