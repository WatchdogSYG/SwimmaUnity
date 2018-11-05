using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puffer : MonoBehaviour {

	public Activator activator;
	public Animator animator;
	private Camera c;

	private float trueFPS, defaultFPS, lastAnimCycle;
	private readonly float animFrames = 4f;
	public float baseMoveSpeed;

	// Use this for initialization
	void Start() {
		activator = gameObject.GetComponent<Activator>();
		animator = gameObject.GetComponent<Animator>();
		c = UnityEngine.Camera.main;
		trueFPS = 3f;
		defaultFPS = 12f;
		animator.speed = trueFPS / defaultFPS;
		baseMoveSpeed = 2f;
		lastAnimCycle = Time.time;
	}

	// Update is called once per frame
	void Update() {
		if (activator.active) {
			Move();
		}
		//check if it has gone being the player's screen bounds
		if (gameObject.transform.position.x > c.ScreenToWorldPoint(new Vector3(Screen.width, 0f)).x) {
			Deactivate();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().TakeDamage(1f);
		}
	}

	void Deactivate() {
		gameObject.GetComponent<Activator>().active = false;
		transform.position = new Vector3(-100f, -100f);//Note this is on the nexagive axes because the world bound checks will check for posistive coords
	}

	void Move() {
		//Firstly get which frame we are on, the animation always starts at frame0
		float now = Time.time;
		float dt = now - lastAnimCycle;//the time since the last frame0
		float elapsedFrames = dt * trueFPS;//the number of frames since the last Update()d frame0;
		elapsedFrames = elapsedFrames % animFrames;//if its above animFrames, the anim has already gone through a cycle, drop that cycle from the calcs and drop the fractional part of the float -> int
		int currentFrame = (int)elapsedFrames;
		
		switch (currentFrame) {
			case 0://we have just deflated, stop, do not drift
				break;
			case 3://we are inflating, slow down
				transform.position += new Vector3(baseMoveSpeed / 2f, 0f) * Time.deltaTime;
				break;
			default://we are in other states(deflating), move normally
				transform.position += new Vector3(baseMoveSpeed, 0f) * Time.deltaTime;
				break;
		}

		//same logic as stingray y changes
		GameObject player = GameObject.FindWithTag("Player");

		//follow the player when score is higher than 100 - sort of
		//if player is behind enemy, enemy will ignore player
		if (player.transform.position.x > gameObject.transform.position.x) {
			//we are in front of the player
			if (player.GetComponent<PlayerController>().score > 100f) {
				//the player has >100 score, follow
				if (player.transform.position.y > gameObject.transform.position.y) {
					transform.position += new Vector3(0f, baseMoveSpeed) * Time.deltaTime;
				}
				else if (player.transform.position.y < gameObject.transform.position.y) {
					transform.position += new Vector3(0f, -baseMoveSpeed) * Time.deltaTime;
				}
				else {
					transform.position += new Vector3(0f, 0f) * Time.deltaTime;
				}
			}
		}
	}
}
