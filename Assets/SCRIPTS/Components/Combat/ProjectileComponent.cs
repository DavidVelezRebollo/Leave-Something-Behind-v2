using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Combat {
	[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
	public class ProjectileComponent : MonoBehaviour {
		[SerializeField] private Projectile BaseStats;
		[SerializeField] private Projectile CurrentStats;

		private float _deltaAir;

		private void Start() {
			_deltaAir = CurrentStats.AirTime;
		}

		private void Update() {
			_deltaAir -= Time.deltaTime;
			
			if(_deltaAir <= 0) Destroy(gameObject);
		}

		public void MultiplyDamage(float damage)
        {
			CurrentStats.Damage *= damage;
		}

		public float GetDamage() {
			return CurrentStats.Damage;
		}

		public float GetSpeed() {
			return CurrentStats.Speed;
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			Destroy(gameObject);
		}

		public void Reset() {
			CurrentStats.Damage = BaseStats.Damage;
			CurrentStats.Speed = BaseStats.Speed;
			CurrentStats.AirTime = BaseStats.AirTime;
		}
	}
}
