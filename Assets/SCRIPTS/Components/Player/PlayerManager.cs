using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using LSB.Classes.Player;
using LSB.Components.Combat;
using LSB.Components.Core;
using LSB.Components.Items;
using LSB.Interfaces;
using LSB.Shared;

namespace LSB.Components.Player {
	public class PlayerManager : MonoBehaviour {
		[Header("Player Stats")]
		[SerializeField] private Stats BaseStats;
		[SerializeField] private Stats CurrentStats;

		private CinemachineVirtualCamera _playerCamera;
		private SpriteRenderer _renderer;
		private GameManager _gameManager;

		private IShoot _shoot;
		private PlayerMovement _movement;
		private PlayerAnimation _animation;
		private float _currentHp;

		public Action OnTakeDamage;

		private void OnEnable() {
			CurrentStats.MaxHp = BaseStats.MaxHp;
			CurrentStats.Speed = BaseStats.Speed;
			CurrentStats.Damage = BaseStats.Damage;
			
			BackPack.Instance.OnItemInitialize += InitializeStats;
			
			_shoot = GetComponent<PlayerAttack>();
			_animation = new PlayerAnimation(GetComponentInChildren<Animator>());
			_playerCamera = FindObjectOfType<CinemachineVirtualCamera>();
			_renderer = GetComponentInChildren<SpriteRenderer>();
			_gameManager = GameManager.Instance;

			_playerCamera.Follow = transform;
		}

		private void InitializeStats() {
			_movement = new PlayerMovement(GetComponent<Rigidbody2D>(), CurrentStats.Speed);
			_currentHp = CurrentStats.MaxHp;
		}

		private void Update() {
			if (_gameManager.GameEnded() || _gameManager.GamePaused()) return;
			_shoot.TickUpdate();
			_animation.TickUpdate();
		}

		private void FixedUpdate() {
			if (_gameManager.GameEnded() || _gameManager.GamePaused()) return;
			_movement.Move();
		}

		private void LateUpdate() {
			if (_currentHp > 0) return;
			
			die();
		}

		private void OnCollisionEnter2D(Collision2D col) {
			if(col.collider.CompareTag("EnemyProjectiles")) 
				TakeDamage(col.collider.GetComponent<ProjectileComponent>().GetDamage());
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
			OnTakeDamage?.Invoke();
		}

		private void die() {
			// TODO - Player Die
			_gameManager.SetGameState(GameState.Lost);
			Debug.LogError("TODO - Player die");
		}
		
		public IEnumerator ChangeColor(Color color) {
			_renderer.color = color;
			yield return new WaitForSeconds(0.1f);
			_renderer.color = Color.white;
		}
	}
}
