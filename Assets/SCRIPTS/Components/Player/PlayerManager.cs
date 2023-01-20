using System;
using Cinemachine;
using LSB.Classes.Enemies;
using UnityEngine;
using LSB.Classes.Player;
using LSB.Components.Items;
using LSB.Interfaces;
using LSB.Shared;

namespace LSB.Components.Player {
	public class PlayerManager : MonoBehaviour {
		[Header("Player Stats")]
		[SerializeField] private Stats BaseStats;
		[SerializeField] private Stats CurrentStats;
		
		CinemachineVirtualCamera _playerCamera;

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
			
			_playerCamera.Follow = transform;
		}

		private void InitializeStats() {
			_movement = new PlayerMovement(GetComponent<Rigidbody2D>(), CurrentStats.Speed);
			_currentHp = CurrentStats.MaxHp;
		}

		private void Update() {
			_shoot.TickUpdate();
		}

		private void FixedUpdate() {
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
			
			_currentHp -= amount;
			OnTakeDamage?.Invoke();
		}

		private void die() {
			// TODO - Player Die
			Debug.LogError("TODO - Player die");
		}
	}
}
