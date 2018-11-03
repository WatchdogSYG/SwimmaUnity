using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour {

	public Activator activator;
	public Animator animator;

	private float trueFPS, defaultFPS, lastAnimCycle;
	private readonly float animFrames = 5f;
	public float baseMoveSpeed;

	// Use this for initialization
	void Start() {
		activator = gameObject.GetComponent<Activator>();
		animator = gameObject.GetComponent<Animator>();
		trueFPS = 8f;
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
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController>().TakeDamage(1f);
			gameObject.GetComponent<Activator>().active = false;
		}
	}
	
	void Move() {
		//we always drift to the left at 1 unit/s
		//Firstly get which frame we are on, the animation always starts at frame0
		float now = Time.time;
		float dt = now - lastAnimCycle;//the time since the last frame0
		float elapsedFrames = dt * trueFPS;//the number of frames since the last Update()d frame0;
		elapsedFrames = elapsedFrames % animFrames;//if its above animFrames, the anim has already gone through a cycle, drop that cycle from the calcs and drop the fractional part of the float -> int
		int currentFrame = (int)elapsedFrames;
		
		switch (currentFrame) {
			case 0://we have just deflated, stop
				transform.position += new Vector3(-1f, 0f) * Time.deltaTime;
				break;
			case 4://we are inflated, slow down
				transform.position += new Vector3(-1f, baseMoveSpeed / 2f) * Time.deltaTime;
				break;
			default://we are in other states, move normally
				transform.position += new Vector3(-1f, baseMoveSpeed) * Time.deltaTime;
				break;
		}

		// tried to get the frame and set motion in the way that the XNA version did it
		/*src : https://forum.unity.com/threads/getting-the-current-frame-of-an-animation-clip.376561/ */
		/*AnimatorClipInfo[] aci = animator.GetCurrentAnimatorClipInfo(0);
		int currentFrame = (int)(aci[0].weight * (aci[0].clip.length * aci[0].clip.frameRate));
		print(aci[0].weight.ToString() + ", " + currentFrame.ToString("F0") + ", " + aci[0].clip.length.ToString() + ", " + aci[0].clip.frameRate.ToString());
		AnimationState.normalisedTime = 0f;*/
	}
}
