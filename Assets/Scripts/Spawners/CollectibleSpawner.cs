using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour {

	public GameObject[] spawnablePrefabs;
	private GameObject[] spawnables;
	private Camera c;
	private Transform t;

	readonly string[] debugNames = new string[3] { "Bubble", "Gem", "Heart" };
	readonly float[] averageDelay = new float[3] { 5f, 0.5f, 30f };//the average delay between spawns
	readonly float[] rangeDelay = new float[3] { 1f, 0.4f, 0f };//the range in which random times are generated for spawns

	float[] lastSpawn = new float[3] { 10f, 0f, 10f };//set these to the first spawn time - delay

	float[] thisDelay = new float[3];//the delay between the next object

	int[] maxActive = new int[3] { (int)Swimma.Spawning.maxBubble, (int)Swimma.Spawning.maxGem, (int)Swimma.Spawning.maxHeart};

	void Start () {
		//initialise variables
		c = UnityEngine.Camera.main;
		t = gameObject.transform;
		gameObject.transform.position = Vector3.zero + new Vector3(8f, 0f);//Set the reference point at the world spawn
		spawnables = new GameObject[spawnablePrefabs.Length];
		//spawn max items in the initial pool
		for (int i = 0; i < 3; i++) {
			InitialPool(i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Spawn a collectible when we reach the spawn delay
		for (int i = 0; i < 3; i++) {
			if ((Time.time - lastSpawn[i]) >= thisDelay[i]) {
				//try to find a pooled item
				GameObject o = null;
				//this is a bit heavy
				foreach (GameObject obj in GameObject.FindGameObjectsWithTag(debugNames[i])) {
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
				RandomiseSpawnTime(i);
				//Debug.Log("Wait  " + thisDelay[i].ToString("F3") + "s before spawning another " + debugNames[i] + ".");
				//set thie last spwned time to now
				lastSpawn[i] = Time.time;
			}
		}
	}

	void InitialPool(int index) {
		//instantiate and set initial pos
		for (int i = 0; i < maxActive[index]; i++) {
			spawnables[index] = Instantiate(spawnablePrefabs[index], t.position, t.rotation) as GameObject;
		}
	}

	void Spawn(GameObject o) {
		//activate at spawn location, everything spawns in the same height range for simplicity		
		//if this is a gem, chose a colour
		if(o.tag == "Gem") {
			//choose a colour gem and set a score value
			int colorChoice = Random.Range(1, 7);
			KeyValuePair<Color, float> gemKV = Swimma.Score.AssignTier(colorChoice);
			o.GetComponent<SpriteRenderer>().color = gemKV.Key;
			o.GetComponent<Gem>().score = gemKV.Value;
		}
		o.transform.position = gameObject.transform.position + new Vector3(0f, Random.Range(-1f, 1f) * (c.orthographicSize - Swimma.Spawning.GEM_AMPLIUDE));
		o.GetComponent<Activator>().active = true;
	}

	void RandomiseSpawnTime(int index) {
		thisDelay[index] = Random.Range(averageDelay[index] - rangeDelay[index], averageDelay[index] + rangeDelay[index]);
	}
}
