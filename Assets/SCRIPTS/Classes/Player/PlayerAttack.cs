using System;
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
		[Tooltip("Special Projectile which will be fired")] 
		[SerializeField] private GameObject SpecialProjectilePrefab;

		[Space(5)]
		[Header("Stats")]
		[Tooltip("Current stats of the player")]
		[SerializeField] private Stats CurrentStats;
		[Tooltip("Amount of energy necessary to do a special attack")] 
		[SerializeField] private float TotalEnergy;
		[Tooltip("Amount of energy recharged per second")] 
		[SerializeField] private float EnergyPerSecond;

		[Tooltip("Event triggered when the player does a especial attack")]
		public Action OnSpecialAttack;

		#endregion

		#region Private Fields

		private InputHandler _input; // Player's Input
		private float _shootDelta; // Time between shoots
		private float _currentEnergy; // Current energy amount

		#endregion

		#region UnityEvents

		private void Awake() {
			foreach (GameObject projectile in ProjectilePrefabs)
				projectile.GetComponent<ProjectileComponent>().Reset();
			
			SpecialProjectilePrefab.GetComponent<ProjectileComponent>().Reset();
		}

		private void Start() {
			_input = InputHandler.Instance;
			_currentEnergy = TotalEnergy;
			_shootDelta = 0.5f;

			foreach(GameObject projectile in ProjectilePrefabs) 
				projectile.GetComponent<ProjectileComponent>().MultiplyDamage(CurrentStats.Damage);
			
			SpecialProjectilePrefab.GetComponent<ProjectileComponent>().MultiplyDamage(CurrentStats.Damage);
		}
		
		public void TickUpdate() {
			if (_currentEnergy < TotalEnergy) _currentEnergy += EnergyPerSecond;

			if (_input.OnShootButton() && _currentEnergy >= TotalEnergy) {
				_currentEnergy = 0;
				SpecialAttack();
			}
			
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

		public float GetTotalEnergy() {
			return TotalEnergy;
		}

		public float GetCurrentEnergyAmount() {
			return _currentEnergy;
		}

		public float GetEnergyPerSeconds() {
			return EnergyPerSecond;
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

		private void SpecialAttack() {
			OnSpecialAttack?.Invoke();
			
			Vector3 position = transform.position;
			Vector2 mousePosition = _input.GetMousePosition();
			
			Vector2 dir = mousePosition - new Vector2(position.x, position.y);
			Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x)  * Mathf.Rad2Deg);
			
			GameObject specialProjectile = Instantiate(SpecialProjectilePrefab, position, rotation);
			
			float speed = specialProjectile.GetComponent<ProjectileComponent>().GetSpeed();
			specialProjectile.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
			specialProjectile.GetComponent<SpecialProjectile>().SetTargetPosition(mousePosition);
		}
		
		#endregion

	}
}
