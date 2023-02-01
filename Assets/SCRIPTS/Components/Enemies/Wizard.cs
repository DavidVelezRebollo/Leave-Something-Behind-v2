using System;
using LSB.Classes.Enemies;
using LSB.Classes.State;
using LSB.Interfaces;
using LSB.Shared;
using UnityEngine;

namespace LSB.Components.Enemies {
	[RequireComponent(typeof(DistanceAttack))]
	public class Wizard : MonoBehaviour, IPooledObject {
		[SerializeField] private Stats BaseStats;
		[SerializeField] private Stats CurrentStats;
		[SerializeField] private float StopDistance = 5f;
		
		private Chase _movement;
		private DistanceAttack _attack;
		
		private Enemy _enemy;

		private void OnEnable() {
			_movement = new Chase(transform, GetComponent<Rigidbody2D>(), CurrentStats.Speed);
			_attack = GetComponent<DistanceAttack>();

			_enemy = new Enemy(_movement, _attack, transform, GetComponentInChildren<Animator>(), GetComponentInChildren<SpriteRenderer>(), 
				gameObject, CurrentStats.MaxHp);
			
			_enemy.SetCurrentState(new Chasing(_enemy, transform, StopDistance, GetComponent<Rigidbody2D>()));

			_attack.SetDamage(CurrentStats.Damage);
		}
		
		public bool Active {
			get => gameObject.activeSelf;
			set => gameObject.SetActive(true);
		}
		
		public void Reset() {
			
		}

		public IPooledObject Clone() {
			GameObject wizardGo = Instantiate(gameObject);
			Wizard wizard = wizardGo.GetComponent<Wizard>();
			return wizard;
		}
		
		private void Update() {
			_enemy.Animate();
		}

		private void FixedUpdate() {
			_enemy.GetCurrentState().FixedUpdate();
		}
		
		public void SubscribeEvent(Action<GameObject> function) {
			_enemy.OnEnemyDie += function;
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			if (_enemy.OnCollide(collision)) StartCoroutine(_enemy.ChangeColor(Color.red));
		}
		
		public void ResetStats() {
			CurrentStats.Damage = BaseStats.Damage;
			CurrentStats.Speed = BaseStats.Speed;
			CurrentStats.MaxHp = BaseStats.MaxHp;
		}
	}
}
