using LSB.Input;
using UnityEngine;

namespace LSB.Classes.Player {
	public class PlayerAnimation {
		private readonly Animator _animator;
		private readonly InputHandler _input;
		
		private Vector2 _moveDirection;
		private Vector2 _lastMoveDirection;
		
		private static readonly int XDirection = Animator.StringToHash("XDirection");
		private static readonly int Horizontal = Animator.StringToHash("Horizontal");
		private static readonly int MovementAmount = Animator.StringToHash("MovementAmount");

		public PlayerAnimation(Animator animator) {
			_animator = animator;
			_input = InputHandler.Instance;
		}

		public void TickUpdate() {
			if (_moveDirection.x != 0 || _moveDirection.y != 0)
				_lastMoveDirection = _moveDirection;

			_moveDirection = new Vector2(_input.GetMovement().x, _input.GetMovement().y).normalized;

			Animate();
		}

		private void Animate() {
			Vector2 movement = _input.GetMovement();

			_animator.SetFloat(XDirection, _lastMoveDirection.x);
			_animator.SetFloat(Horizontal, movement.x);
			_animator.SetFloat(MovementAmount, Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.y)));
		}
	}
}
