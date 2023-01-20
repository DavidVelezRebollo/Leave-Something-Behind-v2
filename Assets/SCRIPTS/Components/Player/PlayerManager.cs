using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using LSB.Classes.Player;
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
		private float _currentHp;

		public Action OnTakeDamage;

		private void OnEnable() {
			CurrentStats.MaxHp = BaseStats.MaxHp;
			CurrentStats.Speed = BaseStats.Speed;
			CurrentStats.Damage = BaseStats.Damage;
			
			BackPack.Instance.OnItemInitialize += InitializeStats;
			
			_shoot = GetComponent<PlayerAttack>();
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
		}

		private void FixedUpdate() {
			if (_gameManager.GameEnded() || _gameManager.GamePaused()) return;
			_movement.Move();
		}

		private void OnCollisionEnter2D(Collision2D col) {
			if (!col.collider.CompareTag("Enemy")) return;
			
			takeDamage(5f);
		}

		private void takeDamage(float amount) {
			if (_currentHp <= 0) {
				die();
				return;
			}
			
			StartCoroutine(ChangeColor(Color.red));
			_currentHp -= amount;
			OnTakeDamage?.Invoke();
		}

		private void die() {
			// TODO - Player Die
			Debug.LogError("TODO - Player die");
		}
		
		public IEnumerator ChangeColor(Color color) {
			_renderer.color = color;
			yield return new WaitForSeconds(0.1f);
			_renderer.color = Color.white;
		}
	}
}
