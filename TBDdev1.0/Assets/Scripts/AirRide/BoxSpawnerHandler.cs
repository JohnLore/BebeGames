using UnityEngine;
using UnityEngine.Networking;
//using System.Collections;

class BoxSpawnerHandler : NetworkBehaviour
{
	public GameObject boxPrefab;

	public uint secondsBetweenSpawn;
	
	public void Update()
	{
		if (((uint)(Time.time) % secondsBetweenSpawn) == 0)
			Spawn ();
	}
			
			
	public void Spawn()
	{
		Vector3 spawnPos = GetSpawnPoint ();
		Quaternion spawnRot = new Quaternion (0f, Random.Range (0, 90), 0f, 0f);
		GameObject box = (GameObject)Instantiate(boxPrefab, spawnPos, spawnRot);
		//tree.GetComponent<Tree>().numLeaves = Random.Range(10,200);
		NetworkServer.Spawn(box);
	}

	public Vector3 GetSpawnPoint()
	{
		float yPos = 10f;
		float xPos = Random.Range (-50, 50);
		float zPos = Random.Range (-50, 50);
	
		return new Vector3 (xPos, yPos, zPos);
	}
}