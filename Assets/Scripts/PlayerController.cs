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
	float gameTime, currentTime;

	public Text ScoreText;
	public Slider AirBar;
	public Slider GameTimeBar;
	public GameObject heartBar;
	[SerializeField]
	public Sprite sp;
	[SerializeField]
	public Canvas c;
	private Camera cam;


	// Use this for initialization
	void Start () {
		cam = UnityEngine.Camera.main;
		score = 0f;
		maxAir = AirBar.maxValue;
		air = maxAir;
		airDrainRate = Swimma.Player.AIR_DRAIN_RATE;
		health = Swimma.Player.MAX_LIVES;
		iTime = 2f;//number of seconds to be invincible for
		invincible = false;
		hitTime = 0f;

		//choose gender
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
		//Drift with the current and move with user input
		Move(Swimma.Movement.PLAYER_MOVEMENT_TYPE, Swimma.Environment.DRIFT_SPEED, Mathf.Cos(Swimma.Environment.DRIFT_ANGLE), Mathf.Sin(Swimma.Environment.DRIFT_ANGLE));
		//Swimma.Movement.Drift(gameObject.transform, Swimma.Environment.DRIFT_SPEED);
		Move(Swimma.Movement.PLAYER_MOVEMENT_TYPE, Swimma.Movement.PLAYER_MOVE_SPEED, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		//update gameTime value;
		currentTime = Time.time - gameTime;
		//update UI tied to player
		ScoreText.text = "Score: " + score.ToString() + "\nHP = " + health.ToString() + "\nTime = " + currentTime.ToString("F2") + "s.";
		GameTimeBar.value = currentTime;
		//drain air and update ui
		RefilAir(-airDrainRate * Time.deltaTime);
		//check if we need to stop being invincible
		if(invincible && ((Time.time - hitTime) > iTime)){
			invincible = false;
			gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

	void StartGameTime() {
		gameTime = Time.time;
	}

	/*
	 * Moves the GameObject this is attached to.
	 * 
	 * @param string movementScheme		Determines the movement type. Either: original janky movement or new smooth movement.
	 * @param v							The Speed that the gameobject will move at.
	 * @param x							The x component of velocity (normalised).
	 * @param y							The y component of velocity (normailsed).
	*/
	public void Move(string movementScheme, float v, float x, float y) {
		Vector3 initialPos = gameObject.transform.position;//for worldbound checking later

		//check the direction of the player input
		if ( x < 0f) {
			gameObject.GetComponent<SpriteRenderer>().flipX = true;
		} else {
			gameObject.GetComponent<SpriteRenderer>().flipX = false;
		}

		//move the player
		if (movementScheme == "Original") {
			transform.position += v * Time.deltaTime * Vector3.Normalize(new Vector3(x, y));
		} else if (movementScheme == "Smooth") {
			transform.position += v * Time.deltaTime * new Vector3(x, y);
		}

		//keep the player in bounds
		//get the dimensions of the screen in world space
		Vector3 worldBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
		Vector3 player = gameObject.transform.position;
		if ((/*left*/player.x < -worldBounds.x) || (/*right*/player.x > worldBounds.x ) || (/*up*/player.y > worldBounds.y) || (/*down*/player.y < -worldBounds.y)) {
			gameObject.transform.position = initialPos;
		}
	}

	public void TakeDamage(float damage) {
		//i made a mistake assuming you had a limited amount of hearts, but it's too late to change that implementation for this assignment
		if (damage < 0f) {
			//we should be able to heal without losing iFrames. In the previous version collecting a heart would end your invincibility time and you would often accidentally run into another enemy too quickly.
			if (health - damage >= Swimma.Player.MAX_LIVES) {
				health = Swimma.Player.MAX_LIVES;
			} else {
				health -= damage;
			}
			//update UI
			heartBar.GetComponent<HeartManager>().UpdateHealth(health);
		} else {
			if (!invincible) {
				//take damage
				health -= damage;
				//update UI
				heartBar.GetComponent<HeartManager>().UpdateHealth(health);
				//start iframes
				invincible = true;
				hitTime = Time.time;
				gameObject.GetComponent<SpriteRenderer>().color = Color.black;
			}
		}
		
		
	}

	public void RefilAir(float amount) {
		air += amount;
		if (air > maxAir) {
			air = maxAir;
		}
		AirBar.value = air;
	}
}
