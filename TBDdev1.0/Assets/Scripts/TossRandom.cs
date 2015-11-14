using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class TossRandom : NetworkBehaviour {
	
	public GameObject[] items;
	private List<GameObject> thrownItems;
	public float throwDistance;
	private int index;

	void Start()
	{
		index = 0;
		thrownItems = new List<GameObject> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (!isLocalPlayer) 
		{
			return;
		}
		if (Input.GetKeyDown("e")) 
		{
			GameObject throwItem = Instantiate (items[index]);
			index++;
			if (index >= items.Length)
			{
				index = 0;
			}
			throwItem.AddComponent<Item_SyncPosition>();
			throwItem.AddComponent<Rigidbody>();
			throwItem.AddComponent<SphereCollider>();
			throwItem.transform.localScale = new Vector3(5, 5, 5);
			throwItem.transform.position = transform.position;
			throwItem.transform.Translate(0, 0, throwDistance);
			thrownItems.Add (throwItem);
			while (thrownItems.Count > 4)
			{
				GameObject oldItem = thrownItems[0];
				thrownItems.RemoveAt (0);
				Destroy (oldItem);
			}
		}
	}
}