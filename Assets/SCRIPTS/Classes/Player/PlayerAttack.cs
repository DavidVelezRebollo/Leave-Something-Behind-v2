using System;
using LSB.Interfaces;
using LSB.Input;
using UnityEngine;
using LSB.Components.Combat;
using LSB.Shared;
using UnityEngine.Serialization;

namespace LSB.Classes.Player {
	public class PlayerAttack : MonoBehaviour {
		private InputHandler _input;
		[SerializeField] private GameObject[] ProjectilePrefabs;
		[SerializeField] private GameObject[] SpecialProjectilesPrefabs;
		[SerializeField] private Stats CurrentStats;

		private float _shootCooldown;
		private float _shootDelta;
		
		private float _specialShootCooldown;
		private float _specialDelta;

		private void Awake() {
			foreach (GameObject projectile in ProjectilePrefabs)
				projectile.GetComponent<ProjectileComponent>().Reset();
		}

		private void Start() {
			_input = InputHandler.Instance;
			_shootDelta = 0.5f;
			_specialDelta = 0f;
			
			foreach(GameObject projectile in ProjectilePrefabs) 
				projectile.GetComponent<ProjectileComponent>().MultiplyDamage(CurrentStats.Damage);
		}

		public GameObject[] GetProyectilePrefab(GameObject newProjectile) {
			return ProjectilePrefabs;
		}

		public void TickUpdate() {
			_shootDelta -= Time.deltaTime;
			if (_shootDelta > 0) return;
			
			Shoot();
		}

		public void Shoot() {
			Vector3 position = transform.position;
			Vector2 mousePosition = _input.GetMousePosition();
			
			int i = 0;
			
			foreach (GameObject proyectile in ProjectilePrefabs) {
				Vector2 dir = mousePosition - new Vector2(position.x + 90*i, position.y + 90*i);
				Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x)  * Mathf.Rad2Deg);
				i++;
				
				GameObject arrow = Instantiate(proyectile, position, rotation);

				float speed = arrow.GetComponent<ProjectileComponent>().GetSpeed();
				arrow.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
			}
			_shootDelta = CurrentStats.AttackCooldown;
		}

		private void specialShoot() {
			
		}
	}
}
