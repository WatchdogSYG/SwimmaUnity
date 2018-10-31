using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	float averageGemDelay;//seconds
	float rangeGemDelay;//should be less than the average
	float thisGemSpawnDelay;
	float lastGemSpawn;

	// Use this for initialization
	void Start () {
		gameObject.transform.position = Vector3.zero;//Set the reference point at the world spawn

		averageGemDelay = 1.5f;
		rangeGemDelay = 1f;
		lastGemSpawn = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		//Spawn a gem when we reach the spawn delay
		if ((Time.time-lastGemSpawn) >= thisGemSpawnDelay) {
			//spawn
			//generate a new spawn delay
			thisGemSpawnDelay = Random.Range(averageGemDelay - rangeGemDelay, averageGemDelay + rangeGemDelay);
			Debug.Log("Wait  " + thisGemSpawnDelay.ToString("F3") + "s before spawning another gem.");
			//set thie last spwned time to now
			lastGemSpawn = Time.time;
		}
	}
}
