using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {

	[SerializeField] Camera ThirdPersonCamera;
	[SerializeField] AudioListener audioListener;

	// Use this for initialization
	void Start () 
	{

		if (isLocalPlayer) 
		{
			GameObject.Find("Scene Camera").SetActive(false);
			GetComponent<CharacterController>().enabled = true;
			GetComponent<PlayerControllerBasic>().enabled = true;
			ThirdPersonCamera.enabled = true;
			audioListener.enabled = true;
	
		}
	}

}
