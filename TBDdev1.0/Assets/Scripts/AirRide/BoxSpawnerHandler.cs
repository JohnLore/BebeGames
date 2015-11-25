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
		/*
		boxSpawns = new GameObject[80*80];
		for (int i = 0; i < 80; i++) 
		{
			for (int j = 0; j < 80; j++) 
			{
				int index = i*80 + j;
				boxSpawns[index] = new GameObject("SpawnPoint" + index);
				boxSpawns[index].transform.position = new Vector3(i-40,10f,j-40);
			}
		}
		*/
		//boxSpawns = GameObject.FindGameObjectsWithTag ("BoxSpawn");
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
			/*
			int randIdx = Random.Range(0, boxSpawns.Length);
			SpawnBox(boxSpawns[randIdx].transform.position);
			*/
			Vector3 spawnPos = new Vector3(Random.Range (-40, 40), 10f, Random.Range (-40, 40));
			SpawnBox(spawnPos);
		}
	}

	void SpawnBox(Vector3 spawnPos)
	{
		count++;
		GameObject go = GameObject.Instantiate (breakableBoxPrefab, spawnPos, Quaternion.identity) as GameObject;
		NetworkServer.Spawn (go);
		//go.GetComponent<Box_ID> ().boxID = "Box" + count;
	}

}