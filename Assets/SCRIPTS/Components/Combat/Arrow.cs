using System;
using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Combat {
	public class Arrow : MonoBehaviour {
		[SerializeField] private Projectile Stats;

		private float _deltaAir;

		private void Start() {
			_deltaAir = Stats.AirTime;
		}

		private void Update() {
			_deltaAir -= Time.deltaTime;
			
			if(_deltaAir <= 0) Destroy(gameObject);
		}

		public void MutiplyDamage(float damage)
        {
			Stats.Damage *= damage;
		}

		public float GetDamage() {
			return Stats.Damage;
		}

		public float GetSpeed() {
			return Stats.Speed;
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			Destroy(gameObject);
		}

        public void ResetStats()
        {
			Stats.Speed = 5;
			Stats.AirTime = 5;
			Stats.Damage = 2;
        }
    }
}
