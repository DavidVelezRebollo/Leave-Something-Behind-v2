using System;
using LSB.Classes.Enemies;
using LSB.Classes.State;
using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Enemies {
	[RequireComponent(typeof(DistanceAttack))]
	public class Wizard : MonoBehaviour {
		[SerializeField] private Stats BaseStats;
		[SerializeField] private Stats CurrentStats;
		[SerializeField] private float StopDistance = 5f;
		
		private Chase _movement;
		private DistanceAttack _attack;
		
		private Enemy _enemy;

		private void OnEnable() {
			resetStats();
			
			_movement = new Chase(transform, GetComponent<Rigidbody2D>(), CurrentStats.Speed);
			_attack = GetComponent<DistanceAttack>();

			_enemy = new Enemy(_movement, _attack, GetComponentInChildren<SpriteRenderer>(), CurrentStats.MaxHp);
			
			_enemy.SetState(new Chasing(_enemy, transform, StopDistance));
			
			_enemy.Start();
		}

		private void FixedUpdate() {
			_enemy.GetCurrentState().FixedUpdate();
		}
		
		public void SubscribeEvent(Action function) {
			_enemy.OnEnemyDie += function;
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			if (_enemy.OnCollide(collision, gameObject)) StartCoroutine(_enemy.ChangeColor(Color.red));
		}
		
		private void resetStats() {
			CurrentStats.Damage = BaseStats.Damage;
			CurrentStats.Speed = BaseStats.Speed;
			CurrentStats.MaxHp = BaseStats.MaxHp;
		}
	}
}
