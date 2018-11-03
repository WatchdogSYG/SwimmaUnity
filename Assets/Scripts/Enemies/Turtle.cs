using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour {

	public Activator activator;
	public Animator animator;

	private float trueFPS, defaultFPS, lastAnimCycle;
	private readonly float animFrames = 4f;
	public float baseMoveSpeed;

	// Use this for initialization
	void Start() {
		activator = gameObject.GetComponent<Activator>();
		animator = gameObject.GetComponent<Animator>();
		trueFPS = 3f;
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

		//same logic as stingray y changes
		GameObject player = GameObject.FindWithTag("Player");

		//EDIT: source seems to suggest this logic, but the exe provided only has the turtle move sideways with no other logic. I will go with the exe "interpretation"
		/*
		//if player is behind enemy, enemy will ignore player
		if (player.transform.position.x < gameObject.transform.position.x) {
			//we are in front of the player
			if (player.transform.position.y > gameObject.transform.position.y) {
				transform.position += new Vector3(-baseMoveSpeed, baseMoveSpeed) * Time.deltaTime;
			}
			else if (player.transform.position.y < gameObject.transform.position.y) {
				transform.position += new Vector3(-baseMoveSpeed, -baseMoveSpeed) * Time.deltaTime;
			}
			else {
				transform.position += new Vector3(-baseMoveSpeed, 0f) * Time.deltaTime;
			}
		} else {
			//we are behind player, cruise
			transform.position += new Vector3(-baseMoveSpeed, 0f) * Time.deltaTime;
		}
		*/
		transform.position += new Vector3(-baseMoveSpeed, 0f) * Time.deltaTime;
	}
}
