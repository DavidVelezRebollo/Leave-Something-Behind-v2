using LSB.Components.Core;
using UnityEngine;

namespace LSB.Classes.Enemies {
	public class EnemyAnimation {
		#region Private Fields

		private readonly Animator _animator; // Animator Component of the enemy
		
		private Vector2 _moveDirection; // Move direction of the enemy
		private Vector2 _lastMoveDirection; // Where the enemy moves at its last movement
		private readonly Enemy _enemy; // Attributes of the enemy
		private GameObject _enemyGameObject;
		private bool _dying;
		
		private static readonly int XDirection = Animator.StringToHash("XDirection"); // XDirection Parameter in the Animator
		private static readonly int Horizontal = Animator.StringToHash("Horizontal"); // Horizontal Parameter in the Animator
		private static readonly int IsStop = Animator.StringToHash("IsStop"); // MovementAmount Parameter in the Animator
		private static readonly int Attack = Animator.StringToHash("Attack"); // IsAttacking Parameter in the Animator
		private static readonly int Die = Animator.StringToHash("Die"); // IsAttacking Parameter in the Animator

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes the Animator Handler of an enemy
		/// </summary>
		/// <param name="animator">Animator Component of the enemy</param>
		/// <param name="enemy">Attributes of the enemy</param>
		public EnemyAnimation(Animator animator, Enemy enemy) {
			_animator = animator;
			_enemy = enemy;

			_enemy.OnEnemyDie += onEnemyDie;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Update of the Animator Handler
		/// </summary>
		public void TickUpdate() {
			if (_dying) {
				if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Die")) return;
				if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
					Object.Destroy(_enemyGameObject);
				return;
			}
			
			if (_moveDirection.x != 0 || _moveDirection.y != 0)
				_lastMoveDirection = _moveDirection;
			
			_moveDirection = clamp(_enemy.GetLastDirection());

			if (GameManager.Instance.GamePaused()) {
				_animator.SetBool(IsStop, true);
				return;
			}
			
			animate();
		}
		
		/// <summary>
		/// Executes the enemy attack animation
		/// </summary>
		public void AttackAnimation() {
			_animator.SetTrigger(Attack);
		}

		#endregion

		#region Auxiliar Methods

		/// <summary>
		/// Animates the enemy
		/// </summary>
		private void animate() {
			
			_animator.SetFloat(XDirection, _lastMoveDirection.x);
			_animator.SetFloat(Horizontal, _moveDirection.x);
			_animator.SetBool(IsStop, _enemy.IsStopped());
		}

		private void onEnemyDie(GameObject enemy) {
			_animator.SetTrigger(Die);
			_enemyGameObject = enemy;
			_dying = true;
		}

		/// <summary>
		/// Clamps the X component of a Vector2 between -1 and 1
		/// </summary>
		/// <param name="clamp">The Vector2 to clamp</param>
		/// <returns></returns>
		private Vector2 clamp(Vector2 clamp) {
			Vector2 vectorClamped = new Vector2(-1,0);
			
			if (clamp.x > 0f) vectorClamped.x = 1;

			return vectorClamped;
        }

		#endregion

	}
}
