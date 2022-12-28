using UnityEngine;

namespace LSB.Shared {
	[CreateAssetMenu(fileName = "Projectile", menuName = "Game/Projectile")]
	public class Projectile : ScriptableObject {
		[Tooltip("Damage of the projectile")]
		public float Damage;
		[Tooltip("Speed of the projectile")]
		public float Speed;
		[Tooltip("Time before the projectile destroys itself")]
		public float AirTime;
	}
}
