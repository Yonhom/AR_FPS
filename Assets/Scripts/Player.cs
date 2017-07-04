
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

	// indicator for the status of the player
	[SyncVar] // this will be sync to all clients for server
	private bool _isDead = false;
	// getter and setter for _isDead
	public bool isDead { 
		get { return _isDead; }
		set { _isDead = value; }
	}
	
	private const int maximumHealth = 100;

	// once this info is changed in local client, it will be automatically synced to all clients through server
	// this variable has to be changed through server. If local client change this variable, won't sync
	// local client can send a command to server, in this command, it variable is changed, then all clients will be synced
//	[SyncVar] // this is not nessessory right now, cause the current health is updated in client rpc
	private int currentHealth = 100;

	[SerializeField]
	private Behaviour[] componentsToDisableWhenDead;

	[SerializeField]
	private float respawmTime = 3.0f;

	// restore to initial state when awake or respawn
	private void SetDefaults() {
		_isDead = false;
		currentHealth = maximumHealth;

		// enable relevant components disabled from death
		for (int i = 0; i < componentsToDisableWhenDead.Length; i++) {
			componentsToDisableWhenDead [i].enabled = true;
		}
		// collider dont fit in behaviour array, enable seperately
		Collider col = GetComponent<Collider>();
		if (col != null)
			col.enabled = true;
	}

	[ClientRpc] // server --> clients
	public void RpcTakeShot(int damage) {

		// if already dead, dont take shot
		if (isDead)
			return;

		currentHealth -= damage;

		Debug.Log(transform.name + " has been shot! " + currentHealth + " health left!"); 

		if (currentHealth <= 0)
			GoDie ();

	}

	private void GoDie () {
		isDead = true;

		Debug.Log (transform.name + " is DEAD!");

		// disable relevent component
		for (int i = 0; i < componentsToDisableWhenDead.Length; i++) {
			componentsToDisableWhenDead [i].enabled = false;
		}
		// collider dont fit in behaviour array, disable seperately
		Collider col = GetComponent<Collider>();
		if (col != null)
			col.enabled = false;

		// respawn
		StartCoroutine(Respawn());
	}

	// respawn after death for 3 seconds
	private IEnumerator Respawn() {
		yield return new WaitForSeconds (respawmTime);

		Transform spawnPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = spawnPoint.position;
		transform.rotation = spawnPoint.rotation;

		isDead = false;

		Debug.Log (transform.name + "is alive again!");

		// set the player to initial state
		SetDefaults ();
			
	}

	public int getMaxmunHealth() {
		return maximumHealth;
	}

	public int getCurrentHealth() {
		return currentHealth;
	}
		
}
