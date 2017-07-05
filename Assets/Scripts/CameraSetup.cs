using UnityEngine;
using UnityEngine.Networking;

/** camera component managment */
public class CameraSetup : NetworkBehaviour {

	// mainly used for remote player's weapon camera component
	[SerializeField]
	private Camera weaponCameraComponent;
	[SerializeField]
	private Camera playerCameraComponent;
	[SerializeField]
	private AudioListener playerCameraAudioListener;

	// Use this for initialization
	void Start () {

		// if object is remote player, diable camera and audio source
		if (!isLocalPlayer) {
			weaponCameraComponent.enabled = false;
			playerCameraComponent.enabled = false;
			playerCameraAudioListener.enabled = false;
		}
		
	}


		

}
