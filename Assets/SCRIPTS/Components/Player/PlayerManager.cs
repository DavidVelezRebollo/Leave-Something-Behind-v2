using UnityEngine;
using LSB.Classes.Player;
using LSB.Interfaces;
using LSB.Shared;

namespace LSB.Components.Player {
	public class PlayerManager : MonoBehaviour {
		[SerializeField] private Stats Attributes;
		[SerializeField] private GameObject ArrowPrefab;
		private IShoot _shoot;
		private PlayerMovement _movement;

		private float _currentHp;

		private void Start() {
			_movement = new PlayerMovement(GetComponent<CharacterController>(), Attributes.Speed);
			_shoot = GetComponent<PlayerAttack>();
			_currentHp = Attributes.MaxHp;
		}

		private void Update() {
			_shoot.TickUpdate();
		}

		private void FixedUpdate() {
			_movement.Move();
		}

		private void takeDamage(float amount) {
			if (_currentHp <= 0) {
				die();
				return;
			}
			
			_currentHp -= amount;
		}

		private void die() {
			// TODO - Player Die
			Debug.LogError("TODO - Player die");
		}
	}
}
