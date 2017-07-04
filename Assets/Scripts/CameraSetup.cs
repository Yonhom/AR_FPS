using UnityEngine;
using UnityEngine.Networking;

public class CameraSetup : NetworkBehaviour {

	// Use this for initialization
	void Start () {

		// if object is remote player, diable camera and audio source
		if (!isLocalPlayer) {
			
			GetComponentInChildren<Camera> ().enabled = false;
			GetComponentInChildren<AudioListener> ().enabled = false;
		}
		
	}

}
