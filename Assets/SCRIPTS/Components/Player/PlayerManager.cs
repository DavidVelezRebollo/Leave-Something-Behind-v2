using UnityEngine;
using LSB.Classes.Player;
using LSB.Interfaces;
using LSB.Shared;

namespace LSB.Components.Player {
	public class PlayerManager : MonoBehaviour {
		public static PlayerManager Instance;
		[SerializeField] private Stats Attributes;
		private IShoot _shoot;
		private PlayerMovement _movement;

		private float _currentHp;

		private void Awake()
		{
			if (Instance != null) return;

			Instance = this;
		}

		private void Start() {
			_movement = new PlayerMovement(GetComponent<CharacterController>(), Attributes.Speed);
			_shoot = GetComponent<PlayerAttack>();
			_currentHp = Attributes.MaxHp;
		}

		public void SetPlayerSpeed(float speed)
        {
			_movement.AddSpeed(speed);
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
