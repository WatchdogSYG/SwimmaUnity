using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

	public float baseMoveSpeed;
	public float airAmount;

	public Activator a;
	private Camera c;

	// Use this for initialization
	void Start () {
		baseMoveSpeed = 2f;
		airAmount = 40f;
		c = UnityEngine.Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
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
			other.gameObject.GetComponent<PlayerController>().RefilAir(airAmount);
			print("Bubble Collected! Air + " + airAmount + ".");
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
