using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class JumpHeightHandler : NetworkBehaviour {
	private bool isHider = false;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer)
			return;
		if (isHider) {
			gameObject.GetComponent<BoxBreakerHandler> ().enabled = false;
			gameObject.GetComponent<PlayerMovementHandler> ().jumpSpeed = 24.0f;
		} 
		else 
		{
			gameObject.GetComponent<BoxBreakerHandler> ().enabled = true;
			gameObject.GetComponent<PlayerMovementHandler> ().jumpSpeed = 8.0f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;
		if (Input.GetKeyUp("q"))
			isHider = !isHider;
	}
}
