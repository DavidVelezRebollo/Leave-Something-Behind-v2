using System;
using LSB.Components.Core;
using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Combat {
	public class ProjectileComponent : MonoBehaviour {
		[SerializeField] protected Projectile BaseStats;
		[SerializeField] protected Projectile CurrentStats;

		protected GameManager GameManager;
		protected Rigidbody2D Rb;
		protected Vector2 Velocity;
		private float _deltaAir;

		private void Start() {
			Rb = GetComponent<Rigidbody2D>();
			GameManager = GameManager.Instance;
			
			Velocity = Rb.velocity;
			_deltaAir = CurrentStats.AirTime;
		}

		protected virtual void Update() {
			if (GameManager.GamePaused() || GameManager.GameEnded()) {
				Rb.velocity = Vector2.zero;
				return;
			}

			Rb.velocity = Velocity;
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
