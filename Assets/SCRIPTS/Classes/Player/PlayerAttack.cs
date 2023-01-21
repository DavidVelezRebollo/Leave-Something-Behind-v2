using LSB.Interfaces;
using LSB.Input;
using UnityEngine;
using LSB.Components.Combat;
using LSB.Shared;

namespace LSB.Classes.Player {
	public class PlayerAttack : MonoBehaviour, IShoot {
		[SerializeField] private float ArrowSpeed = 5f;
		private InputHandler _input;
		[SerializeField] private GameObject ArrowPrefab;
		[SerializeField] private Stats CurrentStats;

		[SerializeField] private float ShootCooldown;
		private float _shootDelta;

        private void Awake()
        {
			ArrowPrefab.GetComponent<Arrow>().ResetStats();
        }
        private void Start() {
			_input = InputHandler.Instance;
			_shootDelta = 0.5f;
			ArrowPrefab.GetComponent<Arrow>().MutiplyDamage(CurrentStats.Damage);
		}

		public void TickUpdate() {
			_shootDelta -= Time.deltaTime;
			if (_shootDelta > 0) return;
			
			Shoot();
		}

		public void Shoot() {
			Vector3 position = transform.position;
			Vector2 mousePosition = _input.GetMousePosition();

			Vector2 dir = mousePosition - new Vector2(position.x, position.y);
			Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x)  * Mathf.Rad2Deg);
			
			GameObject arrow = Instantiate(ArrowPrefab, position, rotation);

			arrow.GetComponent<Rigidbody2D>().velocity = dir.normalized * ArrowSpeed;
			
			_shootDelta = ShootCooldown;
		}
	}
}
