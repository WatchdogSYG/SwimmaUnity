using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float score;
	public float health, iTime, hitTime;
	public bool invincible;
	public float air, maxAir, airDrainRate;
	public char sex;
	public float gameTime;

	public Text t;
	public Slider s;

	[SerializeField]
	public Sprite sp;
	[SerializeField]
	public Canvas c;

	// Use this for initialization
	void Start () {
		score = 0f;
		maxAir = s.maxValue;
		air = maxAir;
		airDrainRate = Swimma.Player.AIR_DRAIN_RATE;
		health = Swimma.Player.MAX_LIVES;
		iTime = 2f;
		invincible = false;

		if (sex == 'M') {
			gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/boy_0") as RuntimeAnimatorController;
		} else if(sex == 'F') {
			gameObject.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Animations/girl_0") as RuntimeAnimatorController;
		}
		Instantiate(sp, c.transform.position, c.transform.rotation);
		StartGameTime();
	}
	
	// Update is called once per frame
	void Update () {
		Swimma.Movement.Drift(gameObject.transform, Swimma.Environment.DRIFT_SPEED);
		Move(Swimma.Movement.PLAYER_MOVEMENT_TYPE);
		t.text = "Score: " + score.ToString() + "\nHP = " + health.ToString() + "\nTime = " + (Time.time - gameTime).ToString("F2") + "s.";

		RefilAir(-airDrainRate * Time.deltaTime);
	}

	void StartGameTime() {
		gameTime = Time.time;
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
		if (!invincible) {
			//take damage
			health -= damage;
			print("Took " + damage + " Damage!");
			//start iframes
			//invincible = true;
		}
		
	}

	public void RefilAir(float amount) {
		air += amount;
		if (air > maxAir) {
			air = maxAir;
		}
		s.value = air;
	}
}
