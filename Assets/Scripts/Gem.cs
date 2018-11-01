using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

	float amplitude;
	float period;//of the wave y(t)

	float phase;//randomise the phase of the wave for each gem

	// Use this for initialization
	void Start () {
		amplitude = Swimma.Spawning.GEM_AMPLIUDE;
		period = Swimma.Spawning.GEM_PEROID;

		phase = Random.Range(0f, 2 * Mathf.PI);
	}
	
	// Update is called once per frame
	void Update () {
		Move();
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
