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
		private float _damageTimer;

		[Header("Stats")]
		[SerializeField] private Stats BaseStats;
		[SerializeField] private Stats CurrentStats;
		[Header("Drop")] 
		[SerializeField] private GameObject PotionPrefab;
		[SerializeField] private GameObject CoinPrefab;
		
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
			_movement = new Chase(transform, GetComponent<Rigidbody2D>(), CurrentStats.Speed);
			_enemy = new Enemy(_movement, _attack, transform, GetComponentInChildren<Animator>(), GetComponentInChildren<SpriteRenderer>(), 
				gameObject, CurrentStats.MaxHp, CurrentStats.AttackCooldown, PotionPrefab, CoinPrefab);
			
			_attack = new MeleeAttack(transform, 0.2f, _enemy.GetAnimation());
		}

		private void Update() {
			_enemy.Animate();
		}

		private void FixedUpdate() {
			_enemy.Move();
		}

		public void SubscribeEvent(Action<GameObject> function) {
			_enemy.OnEnemyDie += function;
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (_enemy.OnTrigger(col)) StartCoroutine(_enemy.ChangeColor(Color.red));
		}
		
		private void OnCollisionEnter2D(Collision2D col) {
			if (_enemy.OnCollide(col)) StartCoroutine(_enemy.ChangeColor(Color.red));
		}
		
		private void OnCollisionStay2D(Collision2D collision) {
			_damageTimer -= Time.deltaTime;

			if (!collision.collider.CompareTag("Player") || _damageTimer > 0) return;
			
			_damageTimer = 0.3f;
			collision.collider.GetComponentInParent<PlayerManager>().TakeDamage(CurrentStats.Damage);
			_enemy.GetAnimation().AttackAnimation();
		}

		private void OnCollisionExit2D(Collision2D other) {
			if (!other.collider.CompareTag("Player")) return;

			_damageTimer = 0f;
		}

		public void ResetStats() {
			CurrentStats.Damage = BaseStats.Damage;
			CurrentStats.Speed = BaseStats.Speed;
			CurrentStats.MaxHp = BaseStats.MaxHp;
			CurrentStats.AttackCooldown = BaseStats.AttackCooldown;
		}

	}
}
