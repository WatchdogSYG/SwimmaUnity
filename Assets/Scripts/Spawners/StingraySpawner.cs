using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingraySpawner : MonoBehaviour {

	readonly float averageDelay = 5f;//the average delay between spawns
	readonly float rangeDelay = 4f;//the range in which random times are generated for spawns
	float lastSpawn = 0f;//set these to the first spawn time - delay
	float thisDelay;//the delay between the next object
	float maxStingray;
	public GameObject stingray;
	private Camera c;

	// Use this for initialization
	void Start() {
		maxStingray = Swimma.Spawning.maxStingray;
		c = UnityEngine.Camera.main;
		InitialPool();
	}

	// Update is called once per frame
	void Update() {
		if ((Time.time - lastSpawn) >= thisDelay) {
			//try to find a pooled item
			GameObject o = null;
			//this is a bit heavy
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Stingray")) {
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
			RandomiseSpawnTime(averageDelay, rangeDelay);

			Debug.Log("Wait  " + thisDelay.ToString("F3") + "s before spawning another Stingray.");
			//set thie last spwned time to now
			lastSpawn = Time.time;
		}
	}

	void Spawn(GameObject obj) {
		//spawns a stingray with randomised y values
		Vector3 here = gameObject.transform.position;
		Debug.Log("Spawn stingray at " + (here + new Vector3(0f, Random.Range(here.y + c.orthographicSize, here.y - c.orthographicSize)).ToString()));
		obj.transform.position = here + new Vector3(0f, Random.Range(here.y + c.orthographicSize, here.y - c.orthographicSize));
		obj.GetComponent<Activator>().active = true;
	}

	void RandomiseSpawnTime(float average, float range) {
		thisDelay = Random.Range(average - range, average + range);
	}

	void InitialPool() {
		for (int i = 0; i < maxStingray; i++) {
			GameObject o = Instantiate(stingray, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		}
	}
}
