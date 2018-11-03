using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

	public float baseMoveSpeed;
	public float airAmount;

	public Activator a;

	// Use this for initialization
	void Start () {
		baseMoveSpeed = 2f;
		airAmount = 40f;
	}
	
	// Update is called once per frame
	void Update () {
		if (a.active) {
			Move();
		} else {
			transform.position = new Vector3(100f, 100f);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		//is it cleaner to use a tag comparison or a try-catch?
		
		try {
			other.gameObject.GetComponent<PlayerController>().RefilAir(airAmount);
			print("40 Air Collected!");
		} catch(System.NullReferenceException e) {
			print("Bubble NullReferenceException: No PlayerController Component found.");
			gameObject.GetComponent<Activator>().active = false;
		}
	}

	void Move() {
		transform.position += new Vector3(-baseMoveSpeed, 0f) * Time.deltaTime;
	}
}
