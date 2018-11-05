using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Slider>().maxValue = Swimma.Score.MAX_LEVEL_TIME;
	}

}
