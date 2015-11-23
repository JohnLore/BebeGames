using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BoxBreakerHandler : NetworkBehaviour {
	// Use this for initialization
	//private MeshRenderer mr;
	void Start () {
		//mr = gameObject.GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision colObj) {
		if (colObj.gameObject.tag == "BreakableBox") {
			//mr.enabled = false;
			CmdPleaseDestroyThisThing(colObj.gameObject);
		}
	}

	[Command]
	void CmdPleaseDestroyThisThing(GameObject thing)
	{
		NetworkServer.UnSpawn (thing);
		NetworkServer.Destroy(thing);
		if (GlobalVars.numBoxesSpawned > 0)
			GlobalVars.numBoxesSpawned--;

		print ("hey look its working");
	}

}

