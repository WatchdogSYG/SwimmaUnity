using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

	private float amplitude;
	private float period;//of the wave y(t)
	private float phase;//randomise the phase of the wave for each gem

	public Activator activator;
	private Camera c;

	public float score;

	// Use this for initialization
	void Start () {
		activator = gameObject.GetComponent<Activator>();
		c = UnityEngine.Camera.main;

		amplitude = Swimma.Spawning.GEM_AMPLIUDE;
		period = Swimma.Spawning.GEM_PEROID;
		phase = Random.Range(0f, 2 * Mathf.PI);
	}
	
	// Update is called once per frame
	void Update () {
		if (activator.active) {
			Move();
		}
		//check if it has gone being the player's screen bounds
		if(gameObject.transform.position.x < c.ScreenToWorldPoint(new Vector3(0f, 0f)).x) {
			Deactivate();
		}
	}

	//if we are collected, add player score, deactivate gem, move to pool in arbitrary location
	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().score += score;
			print("Gem Collected! Score + " + score + ".");
			Deactivate();
		}
	}

	/*
	 * Moves this GameObject to the left at a constant v_x in a sinusoidal path. The amplitude will be randomised to an extent.
	*/
	void Move() {
		float t0 = Time.time;
		float omega = 2 * Mathf.PI / period;
		float vx = Swimma.Environment.DRIFT_SPEED;
		float vy = amplitude * Mathf.Sin(t0 + phase + omega);
		
		transform.position += Time.deltaTime * new Vector3(-vx, vy);
	}

	void Deactivate() {
		gameObject.GetComponent<Activator>().active = false;
		transform.position = new Vector3(100f, 100f);
	}
}
