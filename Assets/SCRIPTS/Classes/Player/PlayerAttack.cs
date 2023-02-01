using System;
using LSB.Interfaces;
using LSB.Input;
using UnityEngine;
using LSB.Components.Combat;
using LSB.Shared;

namespace LSB.Classes.Player {
	public class PlayerAttack : MonoBehaviour, IShoot {
		private InputHandler _input;
		[SerializeField] private GameObject ProjectilePrefab;
		[SerializeField] private Stats CurrentStats;

		private float _shootCooldown;
		private float _shootDelta;

		private void Awake() {
			ProjectilePrefab.GetComponent<ProjectileComponent>().Reset();
		}

		private void Start() {
			_input = InputHandler.Instance;
			_shootDelta = 0.5f;
			ProjectilePrefab.GetComponent<ProjectileComponent>().MultiplyDamage(CurrentStats.Damage);
		}

		public void SetProyectilePrefab(GameObject newProjectile)
        {
			ProjectilePrefab = newProjectile;
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
			
			GameObject arrow = Instantiate(ProjectilePrefab, position, rotation);

			float speed = arrow.GetComponent<ProjectileComponent>().GetSpeed();
			arrow.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
			
			_shootDelta = CurrentStats.AttackCooldown;
		}
	}
}
