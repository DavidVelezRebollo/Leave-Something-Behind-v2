using LSB.Interfaces;
using LSB.Input;
using UnityEngine;

namespace LSB.Classes.Player {
	public class PlayerAttack : MonoBehaviour, IShoot {
		[SerializeField] private float ArrowSpeed = 5f;
		private InputHandler _input;
		[SerializeField] private GameObject ArrowPrefab;

		[SerializeField] private float ShootCooldown;
		private float _shootDelta;

		private void Start() {
			_input = InputHandler.Instance;
			_shootDelta = 0.5f;
		}

		public void TickUpdate() {
			_shootDelta -= Time.deltaTime;
			if (_shootDelta > 0) return;
			
			Shoot();
		}

		public void Shoot() {
			Vector2 dir = (_input.GetMousePosition() - new Vector2(transform.position.x, transform.position.y)).normalized;
			
			GameObject arrow = Instantiate(ArrowPrefab, transform.position, Quaternion.identity);

			arrow.GetComponent<Rigidbody2D>().velocity = dir * ArrowSpeed;
			
			_shootDelta = ShootCooldown;
		}
	}
}
