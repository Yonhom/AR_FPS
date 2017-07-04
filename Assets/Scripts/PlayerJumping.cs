using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(
	typeof(Rigidbody), 
	typeof(ConfigurableJoint)
)]
public class PlayerJumping : NetworkBehaviour {

	[SerializeField]
	private float thrustForce = 1000f;

	private Vector3 jumpForce = Vector3.zero;

	[SerializeField]
	private float maximumJumpingHeight = 10f;

	// add force to rigidbody to jump 
	private Rigidbody rb;
	// disable/enable spring effect when jumping/not jumping using configurable joint
	private ConfigurableJoint configurableJoint;

	[Header("Spring Settings:")]
	[SerializeField]
	private float _positionSpring = 20f;
	[SerializeField]
	private float _maximumForce = 40f;

	void Start() {
		rb = GetComponent<Rigidbody> ();
		configurableJoint = GetComponent<ConfigurableJoint> ();
	}

	void FixedUpdate() {

		// remote player dont update
		if (!isLocalPlayer)
			return;

		// when space bar is keep pressed, first part always return true; sencond expression is for limiting the maximum jumping height
		if (Input.GetKey(KeyCode.Space) && gameObject.transform.position.y < maximumJumpingHeight) {
			// jump force for jumping upward
			jumpForce = Vector3.up * thrustForce;
			// when jump key is pressed, disable spring from dragging player down
			SetSpringSettings(0f);
		} else {
			jumpForce = Vector3.zero;
			// when jump key si not pressed, restore spring
			SetSpringSettings (_positionSpring);
		}

		Jump();
	}

	void Jump() {
		rb.AddForce (
			jumpForce * Time.fixedDeltaTime, 
			ForceMode.Acceleration);
	}

	private void SetSpringSettings(float spring) {
		configurableJoint.yDrive = new JointDrive {
			maximumForce = _maximumForce,
			positionSpring = spring
		};
	}
}
