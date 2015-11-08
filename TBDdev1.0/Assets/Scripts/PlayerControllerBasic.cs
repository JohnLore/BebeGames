using UnityEngine;
using System.Collections;

public class PlayerControllerBasic : MonoBehaviour {

	public GameObject cam;

	void Update() {


		CharacterController controller = GetComponent<CharacterController>();
		PlayerMovementHandler movement = GetComponent<PlayerMovementHandler>();
		//make player move
		controller.Move(movement.getMove(controller, transform.position - cam.transform.position));
	}
}
