using LSB.Classes.Enemies;
using LSB.Components.Items;
using LSB.Shared;
using UnityEngine;
using UnityEngine.AI;

namespace LSB.Components.Enemies {
	public class Orc : MonoBehaviour {
		private Chase _movement;
		private MeleeAttack _attack;
		private NavMeshAgent _agent;

		[SerializeField] private Stats BaseStats;
		[SerializeField] private Stats CurrentStats;

		private Enemy _enemy;

		private void OnEnable() {
			CurrentStats.Damage = BaseStats.Damage;
			CurrentStats.Speed = BaseStats.Speed;
			CurrentStats.MaxHp = BaseStats.MaxHp;

			_movement = new Chase(_agent, CurrentStats.Speed);
			_enemy = new Enemy(_movement, _attack, GetComponentInChildren<SpriteRenderer>(), CurrentStats.MaxHp);
			
			_agent = GetComponent<NavMeshAgent>();
			_attack = new MeleeAttack();

			_enemy.Start();
		}

		private void Update() {
			_enemy.Move();
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			if(_enemy.OnCollide(collision, gameObject)) StartCoroutine(_enemy.ChangeColor(Color.red));
		}

	}
}
