using System;
using LSB.Classes.Enemies;
using LSB.Components.Player;
using LSB.Shared;
using UnityEngine;
using LSB.Components.Combat;

namespace LSB.Components.Enemies {
	public class Orc : MonoBehaviour {
		private Chase _movement;
		private MeleeAttack _attack;

		[SerializeField] private Stats BaseStats;
		[SerializeField] private Stats CurrentStats;

		private Enemy _enemy;

		private void OnEnable() {
			_movement = new Chase(transform,  GetComponent<Rigidbody2D>(), CurrentStats.Speed);
			_enemy = new Enemy(_movement, _attack, GetComponentInChildren<SpriteRenderer>(), CurrentStats.MaxHp);
			
			_attack = new MeleeAttack();

			_enemy.Start();
		}

		private void FixedUpdate() {
			_enemy.Move();
		}

		public void SuscribeEvent(Action function) {
			_enemy.OnEnemyDie += function;
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			if (collision.collider.CompareTag("Player")) {
				collision.collider.GetComponentInParent<PlayerManager>().TakeDamage(CurrentStats.Damage);
				return;
			}

			if (_enemy.OnCollide(collision, gameObject))
			{
				StartCoroutine(_enemy.ChangeColor(Color.red));
				_enemy.TakeDamage(collision.collider.GetComponentInParent<Arrow>().GetDamage());
			}
		}

		

	}
}
