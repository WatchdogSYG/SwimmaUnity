using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySpawner : MonoBehaviour {

	float maxJelly;
	public GameObject jelly;
	private Camera c;
	float width;
	float spawnHeightDistr;

	readonly float averageDelay = 15f;//the average delay between spawns
	readonly float rangeDelay = 5f;//the range in which random times are generated for spawns
	int jellyCount;//number of jellies spawned in current wave
	readonly float waveAverage = 0.5f;//the av spawn delay in a wave
	readonly float waveRange = 0.35f;//the range of spawn delays in a spawn wave
	float lastSpawn = 0f;//set these to the first spawn time - delay
	float thisDelay;//the delay between the next object

	// Pool nJellies = maxJellies together horizontally distributed around the spawner 
	void Start () {
		maxJelly = Swimma.Spawning.maxJelly;
		jellyCount = 0;
		c = UnityEngine.Camera.main;
		//gameObject.transform.position = new Vector3(0f, -c.orthographicSize);
		width = 2 * c.orthographicSize * c.aspect;
		spawnHeightDistr = 1f;
		InitialPool();
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.time - lastSpawn) >= thisDelay) {
			//try to find a pooled item
			GameObject o = null;
			//this is a bit heavy
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Jellyfish")) {
				if (obj.GetComponent<Activator>().active == false) {
					o = obj;
					break;
				}
			}
			//spawn only if theres an inactive obj waiting
			if (o != null) {
				Spawn(o);
			}
			//generate a new spawn delay
			if (jellyCount<maxJelly) {
				//we are still in a spawnwave, use small delays
				RandomiseSpawnTime(waveAverage, waveRange);
			} else {
				//we have finished the spawn wave, use a longer delay
				jellyCount = 0;
				RandomiseSpawnTime(averageDelay,rangeDelay);
			}
			
			Debug.Log("Wait  " + thisDelay.ToString("F3") + "s before spawning another Jellyfish.");
			//set thie last spwned time to now
			lastSpawn = Time.time;
		}
	}

	void InitialPool() {
		for (int i = 0; i < maxJelly; i++) {
			GameObject o = Instantiate(jelly, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		}
	}

	void Spawn(GameObject obj) {
		//spawns maxJellies Jellyfish evenly horizontally distributed with somewhat randomised y values
		float randomY = Random.Range(-spawnHeightDistr / 2f, spawnHeightDistr / 2f);
		Debug.Log("Spawn Jelly at " + gameObject.transform.position + new Vector3((jellyCount - (maxJelly / 2f) + 1f) * width / (maxJelly), randomY).ToString());
		obj.transform.position = gameObject.transform.position + new Vector3((jellyCount - (maxJelly / 2f) + 1f) * width / (maxJelly), randomY);
		obj.GetComponent<Activator>().active = true;
		jellyCount++;
	}

	void RandomiseSpawnTime(float average, float range) {
		thisDelay = Random.Range(average - range, average + range);
	}
}
