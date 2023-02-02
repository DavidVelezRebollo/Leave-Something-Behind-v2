using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using LSB.Classes.Player;
using LSB.Components.Combat;
using LSB.Components.Core;
using LSB.Components.Items;
using LSB.Shared;

namespace LSB.Components.Player {
	public class PlayerManager : MonoBehaviour {
		[Header("Player Stats")]
		[SerializeField] private Stats BaseStats;
		[SerializeField] private Stats CurrentStats;

		private CinemachineVirtualCamera _playerCamera;
		private SpriteRenderer _renderer;
		private GameManager _gameManager;

		private PlayerAttack _shoot;
		private PlayerMovement _movement;
		private PlayerAnimation _animation;
		private float _currentHp;
		private float _immuneDelta;

		public Action OnHpChange;

		private void OnEnable() {
			CurrentStats.MaxHp = BaseStats.MaxHp;
			CurrentStats.Speed = BaseStats.Speed;
			CurrentStats.Damage = BaseStats.Damage;
			CurrentStats.AttackCooldown = BaseStats.AttackCooldown;
			
			BackPack.Instance.OnItemInitialize += InitializeStats;
			
			_shoot = GetComponent<PlayerAttack>();
			_animation = new PlayerAnimation(GetComponentInChildren<Animator>());
			_playerCamera = FindObjectOfType<CinemachineVirtualCamera>();
			_renderer = GetComponentInChildren<SpriteRenderer>();
			_gameManager = GameManager.Instance;

			_playerCamera.Follow = transform;
		}

		private void InitializeStats() {
			_movement = new PlayerMovement(GetComponent<Rigidbody2D>());
			_currentHp = CurrentStats.MaxHp;
		}

		private void Update() {
			if (_gameManager.GameEnded() || _gameManager.GamePaused()) return;
			_shoot.TickUpdate();
			_animation.TickUpdate();
		}

		private void FixedUpdate() {
			if (_gameManager.GameEnded() || _gameManager.GamePaused()) return;
			_movement.Move(CurrentStats.Speed);
		}

		private void LateUpdate() {
			if (_currentHp > 0) return;
			
			die();
		}

		private void OnCollisionEnter2D(Collision2D col) {
			if(col.collider.CompareTag("EnemyProjectiles")) 
				TakeDamage(col.collider.GetComponent<ProjectileComponent>().GetDamage());
			
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (col.GetComponent<Potion>() != null) {
				recoverHp(col.GetComponent<Potion>().GetRecoveryAmount());
				Destroy(col.gameObject);
			}

			if (col.GetComponent<Coin>() != null) {
				Destroy(col.gameObject);
			}
		}

		private void OnTriggerStay2D(Collider2D other) {
			if (!other.CompareTag("Corruption")) return;
			
			if (_immuneDelta <= 0) {
				TakeDamage(1f);
				_immuneDelta = 0.2f;
			}

			_immuneDelta -= Time.deltaTime;
		}

		private void OnTriggerExit2D(Collider2D other) {
			if (!other.CompareTag("Corruption")) return;

			_immuneDelta = 0f;
		}

		public float GetMaxHp() {
			return CurrentStats.MaxHp;
		}

		public float GetCurrentHp() {
			return _currentHp;
		}

		public void TakeDamage(float amount) {
			StartCoroutine(ChangeColor(Color.red));
			_currentHp -= amount;
			OnHpChange?.Invoke();
		}

		private void recoverHp(float amount) {
			if (_currentHp >= CurrentStats.MaxHp) return;
			
			_currentHp += amount;
			if (_currentHp > CurrentStats.MaxHp) _currentHp = CurrentStats.MaxHp;
			
			OnHpChange?.Invoke();
		}

		private void die() {
			_gameManager.SetGameState(GameState.Lost);
		}
		
		public IEnumerator ChangeColor(Color color) {
			_renderer.color = color;
			yield return new WaitForSeconds(0.1f);
			_renderer.color = Color.white;
		}
	}
}
