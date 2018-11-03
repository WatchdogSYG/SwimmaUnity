using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stingray : MonoBehaviour {

	public Activator activator;
	public Animator animator;

	private float trueFPS, defaultFPS, lastAnimCycle;
	private readonly float animFrames = 7f;
	public float baseMoveSpeed;

	// Use this for initialization
	void Start() {
		activator = gameObject.GetComponent<Activator>();
		animator = gameObject.GetComponent<Animator>();
		trueFPS = 4f;
		defaultFPS = 12f;
		animator.speed = trueFPS / defaultFPS;
		baseMoveSpeed = 2f;
		lastAnimCycle = Time.time;
	}

	// Update is called once per frame
	void Update() {
		if (activator.active) {
			Move();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().TakeDamage(1f);
			gameObject.GetComponent<Activator>().active = false;
		}
	}

	void Move() {
		//we always drift to the left at 2 unit/s
		GameObject player = GameObject.FindWithTag("Player");//ref to player pos, score

		//follow the player when score is higher than 100 - sort of
		//if player is behind enemy, enemy will ignore player
		if (player.transform.position.x < gameObject.transform.position.x) {
			//we are in front of the player
			if (player.GetComponent<PlayerController>().score > 100f) {
				//the player has >100 score, follow
				if (player.transform.position.y > gameObject.transform.position.y) {
					transform.position += new Vector3(-baseMoveSpeed, baseMoveSpeed) * Time.deltaTime;
				} else if(player.transform.position.y < gameObject.transform.position.y) {
					transform.position += new Vector3(-baseMoveSpeed, -baseMoveSpeed) * Time.deltaTime;
				} else {
					transform.position += new Vector3(-baseMoveSpeed, 0f) * Time.deltaTime;
				}
			} else {
				//the player has <100 score, just move horizontally
				transform.position += new Vector3(-baseMoveSpeed, 0f) * Time.deltaTime;
			}
		} else {
			//we are behind or parallel to the player, ignore
			transform.position += new Vector3(-baseMoveSpeed, 0f) * Time.deltaTime;
		}
	}
}
