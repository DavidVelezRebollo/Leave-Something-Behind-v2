using UnityEngine;

namespace LSB.Shared {
	[CreateAssetMenu(fileName = "Projectile", menuName = "Game/Projectile")]
	public class Projectile : ScriptableObject {
		public float Damage;
		public float Speed;
		public float AirTime;
	}
}
