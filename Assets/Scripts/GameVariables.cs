using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swimma {
	public class Environment {
		public const float DRIFT_ANGLE = 135f * Mathf.Deg2Rad;//from right horizontal
		public const float DRIFT_SPEED = 2f;
	}

	public class Movement {
		private static readonly string ORIGINAL = "Original";
		//private static readonly string SMOOTH = "Smooth";

		public static string PLAYER_MOVEMENT_TYPE = ORIGINAL;

		public const float PLAYER_MOVE_SPEED = 12f;
	}

	public class Spawning {
		public const float GEM_AMPLIUDE = 0.3f;
		public const float GEM_PEROID = 1.2f;

		public const float maxJelly = 4f;
		public const float maxStingray = 3f;
		public const float maxPuffer = 3f;
		public const float maxTurtle = 3f;

		public const float maxGem = 30f;
		public const float maxBubble = 3f;
		public const float maxHeart = 2f;
	}

	public class Score {
		public static float persistScore;

		public static Color g_cyan =	new Color(104f/255f, 226f/255f, 204f/255f);
		public static Color g_bronze =	new Color(140f/255f, 58f/255f, 18f/255f);
		public static Color g_white =	new Color(240f/255f,240f/255f,240f/255f);
		public static Color g_yellow =	new Color(226f/255f, 244f/255f, 66f/255f);
		public static Color g_seaweed =	new Color(84f/255f,255f/255f,159f/255f);
		public static Color g_purple =	new Color(185f/255f, 132f/255f, 249f/255f);
		
		//from old source, the gems have an equal chance of spawning each colour
		public static KeyValuePair<Color, float> AssignTier(int roll) {
			switch (roll) {
				case 1: return new KeyValuePair<Color, float>(g_cyan, 1f);
				case 2: return new KeyValuePair<Color, float>(g_bronze, 2f);
				case 3: return new KeyValuePair<Color, float>(g_white, 3f);
				case 4: return new KeyValuePair<Color, float>(g_yellow, 5f);
				case 5: return new KeyValuePair<Color, float>(g_seaweed,5f);
				case 6: return new KeyValuePair<Color, float>(g_purple, 10f);
				default:return new KeyValuePair<Color, float>(Color.black, 0f);
			}
		}

		public static float AssignScore(int roll) {
			switch (roll) {
				case 1: return 1f;
				case 2: return 2f;
				case 3: return 3f;
				case 4: return 5f;
				case 5: return 6f;
				case 6: return 10f;
				default: return 0f;
			}
		}

		public const float MAX_LEVEL_TIME = 100f;
	}

	public class Player {
		public const float MAX_LIVES = 5f;
		public const float AIR_DRAIN_RATE = 6f;//air per sec
	}

	public class UI {

	}
}
