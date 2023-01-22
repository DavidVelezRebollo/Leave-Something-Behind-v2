using UnityEngine;

namespace LSB.Classes.Enemies {
	public class EnemyAnimation {
		private readonly Animator _animator;
		
		private Vector2 _moveDirection;
		private Vector2 _lastMoveDirection;
		private readonly Enemy _enemy;
		
		private static readonly int XDirection = Animator.StringToHash("XDirection");
		private static readonly int Horizontal = Animator.StringToHash("Horizontal");
		private static readonly int MovementAmount = Animator.StringToHash("MovementAmount");

		public EnemyAnimation(Animator animator, Enemy enemy) {
			_animator = animator;
			_enemy = enemy;
		}

		public void TickUpdate() {
			if (_moveDirection.x != 0 || _moveDirection.y != 0)
				_lastMoveDirection = _moveDirection;
			
			_moveDirection = clamp(_enemy.GetLastDirection());
			animate();
		}

		private void animate() {
			
			_animator.SetFloat(XDirection, _lastMoveDirection.x);
			_animator.SetFloat(Horizontal, _moveDirection.x);
			_animator.SetFloat(MovementAmount, Mathf.Clamp01(Mathf.Abs(_enemy.GetLastDirection().x) + Mathf.Abs(_enemy.GetLastDirection().y)));
		}

		private Vector2 clamp(Vector2 clamp)
        {
			Vector2 vectorClamped = new Vector2(-1,0);
			if (clamp.x > 0f) vectorClamped.x = 1;

			return vectorClamped;
        }
	}
}
