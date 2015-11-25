using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BoxBreakerHandler : NetworkBehaviour {
	void Start()
	{
	}

	void Update()
	{
	}

	void OnCollisionEnter(Collision colObj) {
		if (colObj.gameObject.tag == "BreakableBox") 
		{
			CmdPleaseDestroyThisThing(colObj.gameObject);
		}
	}

	[Command]
	void CmdPleaseDestroyThisThing(GameObject thing)
	{
		NetworkServer.UnSpawn (thing);
		NetworkServer.Destroy(thing);
	}
}