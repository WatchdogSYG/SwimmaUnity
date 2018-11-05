using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	public float baseMoveSpeed;
	public float healthAmount;

	public Activator a;
	private Camera c;

	// Use this for initialization
	void Start() {
		baseMoveSpeed = 3f;
		healthAmount = 1f;
		c = UnityEngine.Camera.main;
	}

	// Update is called once per frame
	void Update() {
		if (a.active) {
			Move();
		}
		//check if it has gone being the player's screen bounds
		if (gameObject.transform.position.x < c.ScreenToWorldPoint(new Vector3(0f, 0f)).x) {
			Deactivate();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().TakeDamage(-healthAmount);
			print("Heart Collected! Health + " + healthAmount + ".");
			Deactivate();
		}
	}

	void Move() {
		transform.position += new Vector3(-baseMoveSpeed, 0f) * Time.deltaTime;
	}

	void Deactivate() {
		gameObject.GetComponent<Activator>().active = false;
		transform.position = new Vector3(100f, 100f);
	}
}
