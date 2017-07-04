using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : NetworkBehaviour {

	[SerializeField]
	private float speed = 10f;

	private float rotationSensitivity = Constants.ROTATION_SENSITIVITY;

	[SerializeField]
	private Camera cam;

	// animator is referenced here to apply animation according player's movement
	private Animator animator;

	void Start() {
		SetDefaults ();
	}

	void FixedUpdate() {

		// remote player don't update
		if (!isLocalPlayer)
			return;

		PlayerMovementUpdate ();

		PlayerAnimationUpdate ();

		PlayerRotationUpdate ();

	}

	// field initialization
	void SetDefaults() {
		animator = GetComponent<Animator> ();
	}

	void PlayerMovementUpdate() {
		// direction key
		// to make the diretion change to have a smooth effect for the animator, change GetAxisRaw to GetAxis
		float horizontalDirection = Input.GetAxis ("Horizontal");
		float verticalDirection = Input.GetAxis ("Vertical");

		// player movement
		var direction = new Vector3 (horizontalDirection, 0, verticalDirection);
		gameObject.transform.Translate (direction * speed * Time.fixedDeltaTime);
	}

	void PlayerRotationUpdate() {
		// mouse rotation
		float rotationY = Input.GetAxisRaw("Mouse X");

		// player's rotation
		var playerRotation = new Vector3 (0, rotationY, 0) * rotationSensitivity * Time.fixedDeltaTime;
		gameObject.transform.Rotate (playerRotation);
	}

	void PlayerAnimationUpdate () {
		float verticalDirection = Input.GetAxis ("Vertical");
		// change animation accroding player movement
		// MoveVolecity is variable for the animator's animation blend tree
		animator.SetFloat("MoveVolecity", verticalDirection);
	}
}
