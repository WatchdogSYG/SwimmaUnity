using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePersist : MonoBehaviour {
	[SerializeField]
	private static float persistScore;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}

	public void SaveScore(float score) {
		persistScore = score;
	}

	public float RestoreScore() {
		return persistScore;
	}
}
