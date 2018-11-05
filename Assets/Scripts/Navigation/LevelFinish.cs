using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour {

	private static bool isFinished = false;
	GameObject player;
	PlayerController p;

	// Use this for initialization
	void Start() {
		player = GameObject.FindWithTag("Player");
		p = p.GetComponent<PlayerController>();

		//at the beginning of the level, refil the player's lives and air
		p.RefilAir(p.maxAir - p.air);
		p.TakeDamage(p.health - Swimma.Player.MAX_LIVES);
	}

	// Update is called once per frame
	void Update() {
		if (isFinished) {
			p.driftSpeed = 0f;//this makes it feel a tad more realistic;
			if (gameObject.transform.position.x > 4.1f) {
				gameObject.transform.position += new Vector3(-1f, 0f) * Time.deltaTime;
			}
			foreach(GameObject o in GameObject.FindGameObjectsWithTag("Respawn")) {
				o.SetActive(false);
			}
			if (player.transform.position.x > gameObject.transform.position.x) {
				//load next level
			}
		}
	}

	public static void FinishLevel() {
		Debug.Log("Level Finished");
		isFinished = true;
	}
}
