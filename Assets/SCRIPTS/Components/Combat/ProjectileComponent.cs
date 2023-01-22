using System;
using LSB.Components.Core;
using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Combat {
	[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
	public class ProjectileComponent : MonoBehaviour {
		[SerializeField] private Projectile BaseStats;
		[SerializeField] private Projectile CurrentStats;

		private GameManager _gameManager;
		private Rigidbody2D _rb;
		private float _deltaAir;
		private Vector2 _velocity;

		private void Start() {
			_rb = GetComponent<Rigidbody2D>();
			_gameManager = GameManager.Instance;
			
			_velocity = _rb.velocity;
			_deltaAir = CurrentStats.AirTime;
		}

		private void Update() {
			if (_gameManager.GamePaused() || _gameManager.GameEnded()) {
				_rb.velocity = Vector2.zero;
				return;
			}

			_rb.velocity = _velocity;
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
