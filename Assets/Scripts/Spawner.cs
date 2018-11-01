using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] spawnablePrefabs;
	private GameObject[] spawnables;

	readonly string[] debugNames = new string[7] { "Bubble","Gem", "Heart",  "Jellyfish", "Puffer", "Stingray", "Turtle" };
	readonly float[] averageDelay = new float[7] { 15f, 0.5f, 30f, 15f, 15f, 15f, 15f };//the average delay between spawns
	readonly float[] rangeDelay = new float[7] { 1f, 0.4f, 0f, 5f, 5f, 5f, 5f };//the range in which random times are generated for spawns

	bool[] spawnable = Swimma.Spawning.spawnEnabled1;//Sets enemy types for lvl1;
	float[] lastSpawn = new float[7]	{ 0f, 0f, 0f, 0f, 0f, 0f, 0f };//set these to the first spawn time - delay
	
	float[] thisDelay = new float[7];//the delay between the next object

	// Use this for initialization
	void Start () {
		gameObject.transform.position = Vector3.zero + new Vector3(10f, 0f);//Set the reference point at the world spawn
		spawnables = new GameObject[spawnablePrefabs.Length];

		//randomise first spawn times
		for (int i = 0; i < 7; i++) {
			RandomiseSpawnTime(i);
			spawnables[i] = Instantiate(spawnablePrefabs[i]) as GameObject;
		}

	}
	
	// Update is called once per frame
	void Update () {
		//Spawn a gem when we reach the spawn delay
		for(int i = 0; i < 7; i++) {
			if (spawnable[i]) {
				if ((Time.time - lastSpawn[i]) >= thisDelay[i]) {
					//spawn
					Spawn(i);
					//generate a new spawn delay
					RandomiseSpawnTime(i);
					//Debug.Log("Wait  " + thisDelay[i].ToString("F3") + "s before spawning another " + debugNames[i] + ".");
					//set thie last spwned time to now
					lastSpawn[i] = Time.time;
				}
			}
		}
	}

	/*
	 *Sets the next delay of the object ID index to a random number given by averageDelay and rangeDelay
	 * 
	 * @param int index		The object ID to have a time generated.
	 */
	void RandomiseSpawnTime(int index) {
		thisDelay[index] = Random.Range(averageDelay[index] - rangeDelay[index], averageDelay[index] + rangeDelay[index]);
	}

	void Spawn(int index) {
		GameObject o = GameObject.Instantiate(spawnables[index],transform.position,transform.rotation) as GameObject;
	}
}
