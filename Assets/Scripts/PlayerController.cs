using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float score;
	public float health;
	public float air, maxAir, airDrainRate;

	public Text t;
	public Slider s;

	// Use this for initialization
	void Start () {
		score = 0f;
		maxAir = s.maxValue;
		air = maxAir;
		airDrainRate = Swimma.Player.AIR_DRAIN_RATE;
		health = Swimma.Player.MAX_LIVES;
	}
	
	// Update is called once per frame
	void Update () {
		Swimma.Movement.Drift(gameObject.transform, Swimma.Environment.DRIFT_SPEED);
		Move(Swimma.Movement.PLAYER_MOVEMENT_TYPE);
		t.text = "Score: " + score.ToString() + ", HP = " + health.ToString();

		air -= airDrainRate * Time.deltaTime;
		s.value = air;
	}


	/*
	 * Moves the GameObject this is attached to.
	 * 
	 * @param string movementScheme		Determines the movement type. Either: original janky movement or new smooth movement.
	*/
	public void Move(string movementScheme) {
		if (movementScheme == "Original") {
			transform.position += Swimma.Movement.PLAYER_MOVE_SPEED * Time.deltaTime * Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		} else if (movementScheme == "Smooth") {
			transform.position += Swimma.Movement.PLAYER_MOVE_SPEED * Time.deltaTime * new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		}
	}

	public void TakeDamage(float damage) {
		//take damage
		health -= damage;
		print("Took " + damage + " Damage!");
		//start iframes
	}

	public void RefilAir(float amount) {
		air += amount;
		if (air > maxAir) {
			air = maxAir;
		}
	}
}
