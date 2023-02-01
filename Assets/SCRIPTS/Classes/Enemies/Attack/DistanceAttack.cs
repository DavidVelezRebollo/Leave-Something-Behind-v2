using LSB.Components.Combat;
using LSB.Interfaces;
using UnityEngine;

namespace LSB.Classes.Enemies {
    public class DistanceAttack : MonoBehaviour, IAttack {
        #region Serialized Fields

        [Tooltip("Projectile that will be shot")]
        [SerializeField] private GameObject ProjectilePrefab;

        #endregion

        #region Private Fields

        //Damage used to multiply the projectile damage
        private float _enemyDamage;
        //Player position
        private Transform _playerTransform;

        #endregion

        #region Unity Events

        private void Start() {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            ProjectilePrefab.GetComponent<ProjectileComponent>().Reset();
            ProjectilePrefab.GetComponent<ProjectileComponent>().MultiplyDamage(_enemyDamage);
        }

        #endregion

        #region Getters & Setters

        /// <summary>
        /// Sets the enemy distance damage.
        /// </summary>
        /// <param name="damage">The distance damage</param>
        public void SetDamage(float damage) {
            _enemyDamage = damage;   
        }

        #endregion

        #region Interface Methods
        
        public void Attack() {
            Vector3 position = transform.position; // Position where the projectile is shot
            Vector2 dir = _playerTransform.position - position; // Projectile direction
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x)  * Mathf.Rad2Deg); // Rotation of the projectile

            // Creation of the projectile
            GameObject projectile = Instantiate(ProjectilePrefab, position, rotation);
            float speed = projectile.GetComponent<ProjectileComponent>().GetSpeed();
            projectile.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
        }

        #endregion
    }
}

