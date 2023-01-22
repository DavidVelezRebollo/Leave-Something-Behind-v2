using UnityEngine;

namespace LSB.Classes.Enemies {
	public class EnemyAnimation {
		private readonly Animator _animator;
		
		private Vector2 _moveDirection;
		private Enemy _enemy;
		
		private static readonly int XDirection = Animator.StringToHash("XDirection");

		public EnemyAnimation(Animator animator, Enemy enemy) {
			_animator = animator;
			_enemy = enemy;
		}

		public void TickUpdate() {
			_moveDirection = clamp(_enemy.GetLastDirection());
			animate();
		}

		private void animate() {
			_animator.SetFloat(XDirection, _moveDirection.x);
		}

		public void StopAnimation() {
			
		}

		private Vector2 clamp(Vector2 clamp)
        {
			Vector2 vectorClamped = new Vector2(-1,0);
			if (clamp.x > 0f) vectorClamped.x = 1;

			return vectorClamped;
        }
	}
}
