using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	public float baseMoveSpeed;
	public float healthAmount;

	public Activator a;

	// Use this for initialization
	void Start() {
		baseMoveSpeed = 3f;
		healthAmount = 1f;
	}

	// Update is called once per frame
	void Update() {
		if (a.active) {
			Move();
		}
		else {
			transform.position = new Vector3(100f, 100f);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		//is it safer to use a tag comparison or a try-catch?
		try {
			other.gameObject.GetComponent<PlayerController>().TakeDamage(-healthAmount);
			print("Heart Collected!");
		}
		catch (System.NullReferenceException e) {
			print("Heart NullReferenceException: No PlayerController Component found.");
			gameObject.GetComponent<Activator>().active = false;
		}
	}

	void Move() {
		transform.position += new Vector3(-baseMoveSpeed, 0f) * Time.deltaTime;
	}
}
