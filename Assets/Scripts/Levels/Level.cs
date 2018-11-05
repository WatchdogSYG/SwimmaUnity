﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

	private static bool isFinished = false;
	public static bool isCountdown = true;
	public float gameStart;
	GameObject player;
	PlayerController p;

	// Use this for initialization
	void Start() {
		player = GameObject.FindWithTag("Player");
		p = player.GetComponent<PlayerController>();
		gameStart = Time.time;
		//at the beginning of the level, refil the player's lives and air
		p.RefilAir(p.maxAir - p.air);
		p.TakeDamage(p.health - Swimma.Player.MAX_LIVES);/*
		//enable relevant spawners for lvl1 start
		GameObject[] o = GameObject.FindGameObjectsWithTag("Respawn");
		foreach (GameObject obj in o) {
			if (obj.name == "PufferSpawn") {
				obj.SetActive(true);
			}
		}*/
	}

	// Update is called once per frame
	void Update() {
		/*
		//check for other spawn events
		if(Time.time - gameStart > Swimma.Score.MAX_LEVEL_TIME / 2) {
			GameObject[] o = GameObject.FindGameObjectsWithTag("Respawn");
			foreach (GameObject obj in o) {
				if (obj.name == "JellySpawn") {
					obj.SetActive(true);
				}
			}
		}
		*/
		//chec for finished level
		if (isFinished) {
			p.driftSpeed = 0f;//this makes it feel a tad more realistic;
			if (gameObject.transform.position.x > 4.1f) {
				gameObject.transform.position += new Vector3(-1f, 0f) * Time.deltaTime;
			}
			foreach (GameObject o in GameObject.FindGameObjectsWithTag("Respawn")) {
				o.SetActive(false);
			}
			if (player.transform.position.x > gameObject.transform.position.x) {
				//save score and load next level
				Debug.Log(Swimma.Score.persistScore.ToString() + ", " + p.score.ToString());
				GameObject.FindWithTag("Score").GetComponent<ScorePersist>().SaveScore(p.score);
				int scene = SceneManager.GetActiveScene().buildIndex;
				if(scene == 5) {
					SceneManager.LoadScene(2);
				} else {
					SceneManager.LoadScene(scene + 1);
				}
				
			}
		}
	}

	public static void FinishLevel() {
		isFinished = true;
	}
}
