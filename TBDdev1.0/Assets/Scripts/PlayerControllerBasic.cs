using UnityEngine;
using System.Collections;

public class PlayerControllerBasic : MonoBehaviour {

	public float speedF = 6.0F;
	public float speedS = 4.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	public float movementTolerance = 0.01F;

	private Vector3 moveDirection = Vector3.zero;
	private float moveUp = 0;
	public GameObject cam;

	void Update() {
		//Raw Direction Vectors
		Vector3 moveVectorRaw = transform.position - cam.transform.position;
		Vector3 moveForwardRaw = Vector3.ProjectOnPlane (moveVectorRaw, Vector3.up);
		Vector3 moveSideRaw = Vector3.Cross (moveForwardRaw, Vector3.up);

		Vector3 moveForward = Vector3.Normalize (moveForwardRaw);
		Vector3 moveSide = Vector3.Normalize (moveSideRaw);

		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			moveForward *= Input.GetAxisRaw("Vertical");
			moveSide *= - Input.GetAxisRaw("Horizontal");
			/*
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			
			*/
			moveDirection = Vector3.Normalize(moveForward + moveSide);
			if (Vector3.Magnitude (moveDirection) < movementTolerance)
				moveDirection = Vector3.zero;
			
			Vector3 scaleSide = speedS * Vector3.Project (moveDirection, moveSide);
			Vector3 scaleForward = speedF * Vector3.Project (moveDirection, moveForward);
			moveDirection = scaleSide + scaleForward;

			moveUp = 0.0f;
			if (Input.GetButton("Jump"))
				moveUp = jumpSpeed;

		}

		moveUp -= gravity * Time.deltaTime;

		moveDirection.y = moveUp;
		controller.Move(moveDirection * Time.deltaTime);
	}
}
