using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

class BoxSpawnerHandler : NetworkBehaviour
{
	[SerializeField] private GameObject[] boxSpawns;
	private uint count;
	[SerializeField] GameObject breakableBoxPrefab;
	public uint numBoxesPerCycle;
	public uint maxNumBoxes;
	public float spawnCycleSeconds;

	public override void OnStartServer()
	{
		count = 0;
		StartCoroutine (BoxSpawner ());
	}

	IEnumerator BoxSpawner()
	{
		for (;;) 
		{
			yield return new WaitForSeconds(spawnCycleSeconds);
			GameObject[] spawnedBoxes = GameObject.FindGameObjectsWithTag("BreakableBox");
			if (spawnedBoxes.Length < maxNumBoxes)
			{
				CommenceBoxSpawn();
			}
		}
	}

	void CommenceBoxSpawn()
	{
		for (int i = 0; i < numBoxesPerCycle; i++)
		{
			Vector3 spawnPos = new Vector3(Random.Range (-40, 40), 10f, Random.Range (-40, 40));
			Vector3 spawnRot = new Vector3(0f, Random.Range (0, 90), 0f);
			SpawnBox(spawnPos, spawnRot);
		}
	}

	void SpawnBox(Vector3 spawnPos, Vector3 spawnRot)
	{
		count++;
		GameObject go = GameObject.Instantiate (breakableBoxPrefab, spawnPos, Quaternion.identity) as GameObject;
		NetworkServer.Spawn (go);
		go.transform.Rotate (spawnRot);
		//go.GetComponent<Box_ID> ().boxID = "Box" + count;
	}

}