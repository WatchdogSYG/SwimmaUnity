using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Drift(gameObject.transform, Swimma.Environment.DRIFT_SPEED);
		Move(Swimma.Movement.PLAYER_MOVEMENT_TYPE);
	}
	/*
	 * Moves t in the direction specified by Swimma.Environment.DRIFT_ANGLE at a speed of magnitude.
	 * 
	 * @param Transform t		The transform to be moved
	 * @param float Magnitude	The drifting speed
	 */
	void Drift(Transform t, float magnitude) {
		t.position += magnitude * Time.deltaTime * new Vector3(Mathf.Cos(Swimma.Environment.DRIFT_ANGLE), Mathf.Sin(Swimma.Environment.DRIFT_ANGLE));//why do i have to use Vector3
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
}
