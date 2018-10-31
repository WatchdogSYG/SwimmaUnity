using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swimma {
	public class Environment {
		public const float DRIFT_ANGLE = 120f * Mathf.Deg2Rad;//from right horizontal
		public const float DRIFT_SPEED = 1.5f;
	}

	public class Movement {
		private static readonly string ORIGINAL = "Original";
		//private static readonly string SMOOTH = "Smooth";

		public static string PLAYER_MOVEMENT_TYPE = ORIGINAL;

		public const float PLAYER_MOVE_SPEED = 15f;
	}

	public class Collectibles {
		public const float GEM_AMPLIUDE = 0.8f;
		public const float GEM_PEROID = 1.2f;
	}

	public class Enemies {

	}

	public class UI {

	}
}
