using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * @Author Brandon Lu
 * 
 * This class is unused in this build of SwimmaUnity. It spawns GameObjects in a vertical line 
 * at the gameObject location with a line length equal to 2*orthographic size.
 * It can be used in place of the Spawner class for Turtles, Stingrays and Puffers as they all spawn on a vertical line.
 * Feel free to extend this class in a future build.
 */
public class UnusedVerticalSpawner : MonoBehaviour {

	readonly float averageDelay = 5f;//the average delay between spawns
	readonly float rangeDelay = 1f;//the range in which random times are generated for spawns
	float lastSpawn = 0f;//set these to the first spawn time - delay
	float thisDelay;//the delay between the next object

	public GameObject spawnable;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if ((Time.time - lastSpawn) >= thisDelay) {
			//spawn
			Spawn();
			RandomiseSpawnTime(averageDelay, rangeDelay);

			Debug.Log("Wait  " + thisDelay.ToString("F3") + "s before spawning another spawnable.");
			//set thie last spwned time to now
			lastSpawn = Time.time;
		}
	}

	void Spawn() {
		//spawns a spawnable with randomised y values
		Vector3 here = gameObject.transform.position;
		Camera c = UnityEngine.Camera.main;
		Debug.Log("Spawn spawnable at " + (here + new Vector3(0f, Random.Range(here.y + c.orthographicSize, here.y - c.orthographicSize)).ToString()));
		Instantiate(spawnable, here + new Vector3(0f, Random.Range(here.y + c.orthographicSize, here.y - c.orthographicSize)), gameObject.transform.rotation);
	}

	void RandomiseSpawnTime(float average, float range) {
		thisDelay = Random.Range(average - range, average + range);
	}
}
