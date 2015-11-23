using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

class BoxSpawnerHandler : NetworkBehaviour
{
	private uint numSpawned;
	private bool isSpawning;

	public GameObject boxPrefab;

	public uint secondsBetweenSpawn;

	public void Start()
	{
		numSpawned = 0;
		isSpawning = false;
	}

	public void Update()
	{
		if (!isSpawning) 
		{
			isSpawning = true;
			StartCoroutine(Spawn());
		}
	}
			
			
	IEnumerator Spawn()
	{
		Vector3 spawnPos = GetSpawnPoint ();
		Quaternion spawnRot = new Quaternion (0f, Random.Range (0, 90), 0f, 0f);

		yield return new WaitForSeconds(secondsBetweenSpawn);
		GameObject box = (GameObject)Instantiate(boxPrefab, spawnPos, spawnRot);
		//tree.GetComponent<Tree>().numLeaves = Random.Range(10,200);
		NetworkServer.Spawn (box);
		isSpawning = false;
	}
	
	
	public Vector3 GetSpawnPoint()
	{
		float yPos = 10f;
		float xPos = Random.Range (-50, 50);
		float zPos = Random.Range (-50, 50);
	
		return new Vector3 (xPos, yPos, zPos);
	}
}