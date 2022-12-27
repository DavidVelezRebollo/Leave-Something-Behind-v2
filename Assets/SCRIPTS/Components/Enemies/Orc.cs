using System;
using LSB.Classes.Enemies;
using LSB.Shared;
using UnityEngine;
using UnityEngine.AI;

namespace LSB {
	public class Orc : MonoBehaviour {
		private Chase _movement;
		private MeleeAttack _attack;
		private NavMeshAgent _agent;

		[SerializeField] private Stats OrcStats;

		private Enemy _enemy;

		private void Start() {
			_agent = GetComponent<NavMeshAgent>();
			
			_movement = new Chase(_agent, OrcStats.Speed);
			_attack = new MeleeAttack();

			_enemy = new Enemy(_movement, _attack, OrcStats, GetComponent<SpriteRenderer>());
			
			_enemy.Start();
		}

		private void Update() {
			_enemy.Move();
		}

		private void OnCollisionEnter2D(Collision2D col) {
			if (!col.collider.CompareTag("Bullets")) return;

			if (!_enemy.TakeDamage(col.collider.GetComponent<Arrow>().GetDamage())) {
				StartCoroutine(_enemy.ChangeColor(Color.red));
				return;
			}

			_enemy.Die(gameObject);
		}
	}
}
