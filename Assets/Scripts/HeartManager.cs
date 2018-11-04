using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour {

	public GameObject[] hearts;

	// Use this for initialization
	void Start () {
		
	}
	
	/*
	 * Draws (int)health hearts childed to this gameobject in the GameObject array above. If health > the number of hearts drawable, draws all the hearts and prints an error msg.
	 * @param health	The number of hearts to be drawn.
	 */
	public void UpdateHealth(float health) {
		print(health.ToString());
		if ((health < 1) || (health > hearts.Length)) {
			Debug.Log("Health(" + health.ToString() + ") is out of bounds, check logic or UnityEditor.");
		} else {
			for (int i = 0; i < health; i++) {
				hearts[i].GetComponent<Image>().enabled = true;
				print("Enable Heart " + hearts[i].name + i.ToString());
			}
			for (int i = (int)health; i < hearts.Length; i++) {
				hearts[i].GetComponent<Image>().enabled = false;
				print("Disable Heart " + hearts[i].name + i.ToString());
			}
		}
		

		//this was just redundant and expensive
		/*try {
			for (int i = 0; i < health; i++) {
				hearts[i].GetComponent<Image>().enabled = true;
				print("Enable Heart " + hearts[i].name + i.ToString());
			}
			for (int i = (int)health; i < hearts.Length; i++) {
				hearts[i].GetComponent<Image>().enabled = false;
				print("Disable Heart " + hearts[i].name + i.ToString());
			}
		} catch (System.IndexOutOfRangeException e) {
			Debug.Log(e.ToString() + "\nTrying to draw more hearts than possible(" + health.ToString() + "). Check the UnityEditor.");
		} finally {
			if (health > hearts.Length) {
				for (int i = 0; i < hearts.Length; i++) {
					hearts[i].GetComponent<Image>().enabled = true;
				}
			} else {
				for (int i = 0; i < hearts.Length; i++) {
					hearts[i].GetComponent<Image>().enabled = false;
				}
			}
			
		}*/
	}
}
