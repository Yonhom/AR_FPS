using UnityEngine;
using UnityEngine.Networking;

// cause this is a NetworkBehaviour class, we dont have to declare that NetworkIdentity is needed
[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	// the camera to use when no player is spawned yet
	private Camera sceneCamera;

	// layer masking name for remote player
	private const string REMOTE_PLAYER_LAYER = "RemotePlayer";

	// layer masking name for graphics that dont want to be drawn in the scene
	private const string DONT_DRAW_LAYER = "DontDraw";
	[SerializeField]
	private GameObject graphicsWontBeDrawn;

	[SerializeField]
	private Animator playerAnimator;

	[SerializeField]
	private GameObject crossHairPrefab;
	private GameObject crossHair;

	void Start() {

		DisableSceneCamera ();

		if (!isLocalPlayer) { // player on the local machine (network related)
			
			AssignRemoteLayer ();
			// remote player's animator component has to be diabled, or it will override animation tranform info sended from server
			if (playerAnimator != null)
				playerAnimator.enabled = false;
			
		} else { 
			
			// local player dont draw player except the gun
			// why assign layer mask to a game object in code, rather than directionly in the prefabs?
			// cause we dont want all of the game objects spawned by this prefab to be masked, only local game object
			Utils.AssignLayerMaskToObjectAndChildren (graphicsWontBeDrawn, LayerMask.NameToLayer (DONT_DRAW_LAYER));

			// local PLayer add the crosshair UI
			crossHair = Instantiate(crossHairPrefab);
			crossHair.name = crossHairPrefab.name;

		}
			
	}

	// register the player component of the object the current script component attached to
	public override void OnStartClient() { // every time a new client is created, this method is called the player associated with gameobject is registered
		base.OnStartClient();
		registerPlayer ();
	}

	private void registerPlayer() {
		string _playerID = GetComponent<NetworkIdentity> ().netId.ToString ();
		Player _player = GetComponent<Player> ();
		GameManager.registerPlayer (_playerID, _player);
	}
		
	private void unregisterPlayer() {
		GameManager.unregisterPlayer (GetComponent<NetworkIdentity>().netId.ToString());
	}

	// player object is defaulted as a local player, mark remote player as remote player
	private void AssignRemoteLayer() {
		gameObject.layer = LayerMask.NameToLayer(REMOTE_PLAYER_LAYER);
	}

	void DisableSceneCamera() {
		sceneCamera = Camera.main;
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive(false);
		}
	}
		
	void OnDisable() {
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive(true);
		}

		// unregister the object the current script component attached to
		unregisterPlayer();

		// destory crosshair UI
		Destroy(crossHair);
	}
}
