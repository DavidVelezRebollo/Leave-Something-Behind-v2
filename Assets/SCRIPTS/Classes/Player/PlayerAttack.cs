using System.Collections.Generic;
using LSB.Input;
using UnityEngine;
using LSB.Components.Combat;
using LSB.Shared;

namespace LSB.Classes.Player {
	public class PlayerAttack : MonoBehaviour {
		#region Public Fields
		
		[Header("Prefabs")]
		[Tooltip("Projectiles which will be fired")]
		[SerializeField] private List<GameObject> ProjectilePrefabs;

		[Space(5)]
		[Header("Stats")]
		[Tooltip("Current stats of the player")]
		[SerializeField] private Stats CurrentStats;

		#endregion

		#region Private Fields

		private InputHandler _input; // Player's Input
		private float _shootDelta; // Time between shoots

		#endregion

		#region UnityEvents

		private void Awake() {
			foreach (GameObject projectile in ProjectilePrefabs)
				projectile.GetComponent<ProjectileComponent>().Reset();
		}

		private void Start() {
			_input = InputHandler.Instance;
			_shootDelta = 0.5f;

			foreach(GameObject projectile in ProjectilePrefabs) 
				projectile.GetComponent<ProjectileComponent>().MultiplyDamage(CurrentStats.Damage);
		}
		
		public void TickUpdate() {
			_shootDelta -= Time.deltaTime;
			if (_shootDelta > 0) return;
			
			Shoot();
		}
		
		#endregion

		#region Getters & Setters

		/// <summary>
		/// Add a projectile to the projectile's list
		/// </summary>
		/// <param name="newProjectile">The projectile to be added</param>
		public void AddProjectilePrefabs(GameObject newProjectile) {
			ProjectilePrefabs.Add(newProjectile);
		}

		/// <summary>
		/// Removes a projectile from the projectile's list
		/// </summary>
		/// <param name="removedProjectile">The projectile to be removed</param>
		public void RemoveProjectilePrefabs(GameObject removedProjectile) {
			ProjectilePrefabs.Remove(removedProjectile);
		}

		#endregion

		#region Auxiliar Methods

		/// <summary>
		/// Shoots a projectile
		/// </summary>
		private void Shoot() {
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
		
		#endregion

	}
}
