using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void LoadScene() {
		Debug.Log("Loading Scene: \"Lv1_Blue\"");
		SceneManager.LoadScene(2);
		Debug.Log("Done");
	}
}
