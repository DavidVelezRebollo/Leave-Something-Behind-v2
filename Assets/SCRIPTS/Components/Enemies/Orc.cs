using System;
using LSB.Classes.Enemies;
using LSB.Components.Player;
using LSB.Interfaces;
using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Enemies {
	public class Orc : MonoBehaviour, IPooledObject {
		private Chase _movement;
		private MeleeAttack _attack;

		[SerializeField] private Stats BaseStats;
		[SerializeField] private Stats CurrentStats;

		private Enemy _enemy;
		
		public bool Active {
			get => gameObject.activeSelf;
			set => gameObject.SetActive(true);
		}

		public void Reset() {
			
		}

		public IPooledObject Clone() {
			GameObject orcGo = Instantiate(gameObject);
			Orc orc = orcGo.GetComponent<Orc>();
			return orc;
		}

		private void OnEnable() {
			_movement = new Chase(transform,  GetComponent<Rigidbody2D>(), CurrentStats.Speed);
			_enemy = new Enemy(_movement, _attack, GetComponentInChildren<Animator>(), GetComponentInChildren<SpriteRenderer>(), 
				CurrentStats.MaxHp);
			
			_attack = new MeleeAttack();

			_enemy.Start();
		}

		private void FixedUpdate() {
			_enemy.Move();
		}

		public void SubscribeEvent(Action<GameObject> function) {
			_enemy.OnEnemyDie += function;
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			if (collision.collider.CompareTag("Player")) {
				collision.collider.GetComponentInParent<PlayerManager>().TakeDamage(CurrentStats.Damage);
				return;
			}

			if (_enemy.OnCollide(collision, gameObject)) StartCoroutine(_enemy.ChangeColor(Color.red));
		}

		public void ResetStats() {
			CurrentStats.Damage = BaseStats.Damage;
			CurrentStats.Speed = BaseStats.Speed;
			CurrentStats.MaxHp = BaseStats.MaxHp;
		}

	}
}
