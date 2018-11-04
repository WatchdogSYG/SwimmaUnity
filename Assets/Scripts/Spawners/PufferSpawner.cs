using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufferSpawner : MonoBehaviour {

	readonly float averageDelay = 5f;//the average delay between spawns
	readonly float rangeDelay = 1f;//the range in which random times are generated for spawns
	float lastSpawn = 0f;//set these to the first spawn time - delay
	float thisDelay;//the delay between the next object

	public GameObject puffer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.time - lastSpawn) >= thisDelay) {
			//spawn
			Spawn();
			RandomiseSpawnTime(averageDelay, rangeDelay);

			Debug.Log("Wait  " + thisDelay.ToString("F3") + "s before spawning another Puffer.");
			//set thie last spwned time to now
			lastSpawn = Time.time;
		}
	}

	void Spawn() {
		//spawns a Puffer with randomised y values
		Vector3 here = gameObject.transform.position;
		Camera c = UnityEngine.Camera.main;
		Debug.Log("Spawn Puffer at " + (here + new Vector3(0f, Random.Range(here.y + c.orthographicSize, here.y - c.orthographicSize)).ToString()));
		Instantiate(puffer, here + new Vector3(0f, Random.Range(here.y + c.orthographicSize, here.y - c.orthographicSize)), gameObject.transform.rotation);
	}

	void RandomiseSpawnTime(float average, float range) {
		thisDelay = Random.Range(average - range, average + range);
	}
}
