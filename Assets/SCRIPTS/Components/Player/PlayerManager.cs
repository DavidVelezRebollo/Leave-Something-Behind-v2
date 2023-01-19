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
			_shoot = GetComponent<PlayerAttack>();
		}

		public void InitializeStats()
        {
			_movement = new PlayerMovement(GetComponent<CharacterController>(), Attributes.Speed);
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
