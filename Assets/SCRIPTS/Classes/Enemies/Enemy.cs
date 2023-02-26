using System;
using LSB.Components.Combat;
using LSB.Interfaces;
using UnityEngine;
using System.Collections;
using LSB.Components.Core;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
        private readonly GameObject _potionPrefab; // Potion drop GameObject
        private readonly GameObject _coinPrefab; // Coin drop GameObject
        private readonly float _attackCooldown; //Time between attacking
        
        private float _currentHp; // CurrentHP of the enemy
        private float _cooldownDelta; // Used on the shoot cooldown
        private bool _isStopped; // Checks if the enemy is attacking
        private bool _dying; // Checks if the enemy is dead
        private bool _dieSound; // Checks if the enemy is going to do the die sound

        private const float _DROP_PROBABILITY = 0.03f; // Probability for drop a potion

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
        /// <param name="attackCooldown">Time between attacking the player</param>
        /// <param name="potion">Potion Prefab</param>
        /// <param name="coin">Coin Prefab</param>
        public Enemy(IEnemyMove movementType, IAttack attackType, Transform transform, Animator animator, SpriteRenderer renderer, 
            GameObject gameObject, float maxHp, float attackCooldown, GameObject potion, GameObject coin) {
            _movement = movementType;
            _attack = attackType;
            _transform = transform;
            _renderer = renderer;
            _gameObject = gameObject;
            _currentHp = maxHp;
            _attackCooldown = attackCooldown;
            _coinPrefab = coin;
            _potionPrefab = potion;
            
            _gameManager = GameManager.Instance;
            _animation = new EnemyAnimation(animator, this);
            _player = GameObject.FindGameObjectWithTag("Player").transform;

            _cooldownDelta = 0f;
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

            takeDamage(col.collider.GetComponent<ProjectileComponent>().GetDamage());

            return true;
        }

        public bool OnTrigger(Collider2D col) {
            if (!col.CompareTag("Bullets")) return false;

            takeDamage(col.GetComponentInParent<ProjectileComponent>().GetDamage());
            
            return true;
        }

        #endregion

        #region Getters & Setters
        
        public EnemyAnimation GetAnimation() { return _animation; }

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
        public bool IsStopped() { return _isStopped; }

        /// <summary>
        /// Checks if the enemy is going to do the die sound
        /// </summary>
        /// <returns>True if it will do it. False otherwise</returns>
        public bool MakeDeadSound() { return _dieSound; }

        #endregion

        #region Methods

        /// <summary>
        /// Moves the enemy. Depends on the type of movement attached to the enemy
        /// </summary>
        public void Move() {
            if (_gameManager.GameEnded() || _gameManager.GamePaused() || _dying) return;
            if (Vector3.Distance(_player.position, _transform.position) >= 10) {
                die(_gameObject, false);
                return;
            }
            
            _movement?.Move(_player.position);
            _isStopped = false;
        }
        
        /// <summary>
        /// Attacks the player. Depends on the type of attack attached to the enemy
        /// </summary>
        public void Attack() {
            if (_gameManager.GameEnded() || _gameManager.GamePaused() || _dying) return;
            // Reduce the cooldown timer.
            _cooldownDelta -= Time.deltaTime;
            if(_cooldownDelta <= 0.8f && _cooldownDelta >= 0.78f) _animation.AttackAnimation();
            if (_cooldownDelta > 0) return;
            
            _attack?.Attack();
            _isStopped = true;
            
            // Cooldown reset
            _cooldownDelta = _attackCooldown;
        }

        public void Animate() {
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
        /// Makes the enemy take a certain amount of damage
        /// </summary>
        /// <param name="amount">The amount of damage</param>
        private void takeDamage(float amount) {
            _currentHp -= amount;

            if (_currentHp <= 0) {
                die(_gameObject, true);
            }
        }

        /// <summary>
        /// Handles what to do when the enemy dies.
        /// </summary>
        /// <param name="enemy">The enemy GameObject</param>
        private void die(GameObject enemy, bool dieSound) {
            _dieSound = dieSound;
            OnEnemyDie?.Invoke(enemy);
            _gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;

            float p = Random.Range(0f, 1f);

            if(p < _DROP_PROBABILITY) {
                Object.Instantiate(_potionPrefab, _transform.position, Quaternion.identity);
            }

            _dying = true;
        }

        #endregion
    }
}
