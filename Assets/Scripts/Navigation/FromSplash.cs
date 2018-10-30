using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromSplash : MonoBehaviour {

	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//on any input, load the main menu
		if (Input.anyKey) {
			Debug.Log("Loading Main Menu...");
			SceneManager.LoadScene(1);
			Debug.Log("Done");
		}
	}
}
