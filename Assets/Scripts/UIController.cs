using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	int sex = 0;//0==boy,1==girl

	public static Text score;
	public static Slider airBar;

	public static void DrawAir(float air) {
		airBar.value = air;
	}

	public static void DrawScore(float score) {
		
	}
}
