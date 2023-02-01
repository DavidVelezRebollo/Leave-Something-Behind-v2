using System;
using LSB.Components.Combat;
using LSB.Interfaces;
using UnityEngine;
using System.Collections;
using LSB.Components.Core;
using Object = UnityEngine.Object;

namespace LSB.Classes.Enemies {
    public class Enemy {
        #region Private Fields

        private readonly IEnemyMove _movement; // Type of movement of the enemy
        private readonly IAttack _attack; // Type of attack of the enemy
        private IState _currentState; // Current state of the enemy, if it uses a FSM

        private readonly GameManager _gameManager; // Instance of the GameManager
        private readonly EnemyAnimation _animation; // Animation Handler of the enemy
        private readonly SpriteRenderer _renderer; // Sprite Renderer Component of the enemy

        private readonly Transform _player; // Player's Transform Component
        private readonly Transform _transform; // Enemy's Transform Component
        private readonly GameObject _gameObject; // Enemy's GameObject
        private float _currentHp; // CurrentHP of the enemy
        private bool _isAttacking;
        private bool _dying;
        
        #endregion

        #region Events

        public Action<GameObject> OnEnemyDie; // Event that occurs when the enemy dies

        #endregion

        #region Constructor
        
        /// <summary>
        /// Initializes an enemy
        /// </summary>
        /// <param name="movementType">Enemy's movement type</param>
        /// <param name="attackType">Enemy's attack type</param>
        /// <param name="transform">Enemy's Transform Component</param>
        /// <param name="animator">Enemy's Animator Component</param>
        /// <param name="renderer">Enemy's SpriteRenderer Component</param>
        /// <param name="gameObject">Enemy's GameObject</param>
        /// <param name="maxHp">Enemy's MaxHp</param>
        public Enemy(IEnemyMove movementType, IAttack attackType, Transform transform, Animator animator, SpriteRenderer renderer, GameObject gameObject, float maxHp) {
            _movement = movementType;
            _attack = attackType;
            _transform = transform;
            _renderer = renderer;
            _gameObject = gameObject;
            _currentHp = maxHp;
            
            _gameManager = GameManager.Instance;
            _animation = new EnemyAnimation(animator, this);
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        #endregion

        #region Event Functions
        // Functions that should be called at Unity's Event Functions
        
        /// <summary>
        /// Handles the enemy collision
        /// </summary>
        /// <param name="col">Object which collided with the enemy</param>
        /// <returns>True if the enemy collides with a bullet and false otherwise</returns>
        public bool OnCollide(Collision2D col) {
            if (!col.collider.CompareTag("Bullets")) return false;

            _currentHp -= col.collider.GetComponent<ProjectileComponent>().GetDamage();

            if (_currentHp <= 0) {
                die(_gameObject);
            }

            return true;
        }

        #endregion

        #region Getters & Setters

        /// <summary>
        /// Gets the current state the enemy is.
        /// </summary>
        /// <returns>The current state</returns>
        public IState GetCurrentState() {
            return _currentState;
        }
        
        /// <summary>
        /// Sets the current state of the enemy
        /// </summary>
        /// <param name="state">The new state</param>
        public void SetCurrentState(IState state) {
            _currentState?.Exit();

            _currentState = state;
            _currentState.Enter();
        }
        
        /// <summary>
        /// Get the last direction the enemy was looking
        /// </summary>
        /// <returns>A Vector2 which indicates the last direction the enemy was looking</returns>
        public Vector2 GetLastDirection() { return _movement.GetLastDirection(); }

        /// <summary>
        /// Checks if the enemy is currently attacking.
        /// </summary>
        /// <returns>True if its attacking. False otherwise</returns>
        public bool IsAttacking() { return _isAttacking; }

        #endregion

        #region Methods

        /// <summary>
        /// Moves the enemy. Depends on the type of movement attached to the enemy
        /// </summary>
        public void Move() {
            if (_gameManager.GameEnded() || _gameManager.GamePaused() || _dying) return;
            if (Vector3.Distance(_player.position, _transform.position) >= 10) {
                die(_gameObject);
                return;
            }
            
            _movement?.Move(_player.position);
            _isAttacking = false;
        }
        
        /// <summary>
        /// Attacks the player. Depends on the type of attack attached to the enemy
        /// </summary>
        public void Attack() {
            if (_gameManager.GameEnded() || _gameManager.GamePaused() || _dying) return;
            _attack?.Attack();
            _isAttacking = true;
        }

        public void Animate() {
            if (_gameManager.GameEnded() || _gameManager.GamePaused()) return;
            _animation.TickUpdate();
        }
        
        public IEnumerator ChangeColor(Color color) {
            _renderer.color = color;
            yield return new WaitForSeconds(0.1f);
            _renderer.color = Color.white;
        }

        #endregion

        #region Auxiliar Methods

        /// <summary>
        /// Handles what to do when the enemy dies.
        /// </summary>
        /// <param name="enemy">The enemy GameObject</param>
        private void die(GameObject enemy) {
            OnEnemyDie?.Invoke(enemy);
            
            _gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            _dying = true;
        }

        #endregion


        
    }
}
