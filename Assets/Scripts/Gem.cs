using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

	float amplitude;
	float period;//of the wave y(t)

	// Use this for initialization
	void Start () {
		amplitude = Swimma.Collectibles.GEM_AMPLIUDE;
		period = Swimma.Collectibles.GEM_PEROID;
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
		float vy = amplitude * Mathf.Sin(t0 + omega);

		transform.position += Time.deltaTime * new Vector3(-vx, vy);
	}
}
