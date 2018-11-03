using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

	private float amplitude;
	private float period;//of the wave y(t)
	private float phase;//randomise the phase of the wave for each gem

	public Activator activator;

	public float score;

	// Use this for initialization
	void Start () {
		activator = gameObject.GetComponent<Activator>();

		amplitude = Swimma.Spawning.GEM_AMPLIUDE;
		period = Swimma.Spawning.GEM_PEROID;
		phase = Random.Range(0f, 2 * Mathf.PI);

		//choose a colour gem and set a score value
		int colorChoice = Random.Range(1,7);
		KeyValuePair<Color, float> gemKV = Swimma.Score.AssignTier(colorChoice);
		gameObject.GetComponent<SpriteRenderer>().color = gemKV.Key;
		score = gemKV.Value;
	}
	
	// Update is called once per frame
	void Update () {
		if (activator.active) {
			Move();
		} else {
			transform.position = new Vector3(100f, 100f);
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().score += score;
			print("Gem Collected! Score + " + score + ".");
			gameObject.GetComponent<Activator>().active = false;
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
}
