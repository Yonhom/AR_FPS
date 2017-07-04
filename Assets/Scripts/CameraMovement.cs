using UnityEngine;
using UnityEngine.Networking;

public class CameraMovement : NetworkBehaviour {

	private Camera cam;

	private float rotationSensitivity = Constants.ROTATION_SENSITIVITY;

	// maximum looking up/down angle
	private float maximumAngle = 85f;

	// the x rotation that the camera needs
	private float currentCameraRotation = 0f;

	void Start() {
		cam = GetComponentInChildren<Camera> ();
	}

	void FixedUpdate() {

		// remote player dont update
		if (!isLocalPlayer)
			return;

		// mouse y is rotation x
		float rotationX = Input.GetAxisRaw("Mouse Y");
		// the x amix rotation of the camera with a manimum angle
		currentCameraRotation -= rotationX;
		currentCameraRotation = Mathf.Clamp (currentCameraRotation, -maximumAngle, maximumAngle);

		// this is the camera's rotation
		if (cam != null) {
			var camRotation = currentCameraRotation * rotationSensitivity * Time.fixedDeltaTime;
			cam.transform.localEulerAngles = new Vector3 (camRotation, 0f, 0f);

		}
	}
}
