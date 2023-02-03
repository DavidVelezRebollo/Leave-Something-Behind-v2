using LSB.Input;
using UnityEngine;

namespace LSB.Classes.Player {
	public class PlayerAnimation {
		#region Private Fields

		private readonly Animator _animator; // Animator of the player.
		private readonly InputHandler _input; // Player's Input
		
		private Vector2 _moveDirection; // Direction where the player moves
		private Vector2 _lastMoveDirection; // Last direction where the player moved
		
		private static readonly int XDirection = Animator.StringToHash("XDirection"); // XDirection Parameter in the Animator
		private static readonly int Horizontal = Animator.StringToHash("Horizontal"); // Horizontal Parameter in the Animator
		private static readonly int MovementAmount = Animator.StringToHash("MovementAmount"); // MovementAmount Parameter in the Animator

		#endregion

		#region Constructor

		/// <summary>
		/// Animates the player
		/// </summary>
		/// <param name="animator">Animator Component of the player</param>
		public PlayerAnimation(Animator animator) {
			_animator = animator;
			_input = InputHandler.Instance;
		}

		#endregion

		#region Methods
		
		/// <summary>
		/// Player Animator's Update. Needs to be call on a MonoBehaviour
		/// </summary>
		public void TickUpdate() {
			if (_moveDirection.x != 0 || _moveDirection.y != 0)
				_lastMoveDirection = _moveDirection;

			_moveDirection = new Vector2(_input.GetMovement().x, _input.GetMovement().y).normalized;

			Animate();
		}

		#endregion

		#region Auxiliar Methods

		/// <summary>
		/// Animates the player
		/// </summary>
		private void Animate() {
			Vector2 movement = _input.GetMovement();

			_animator.SetFloat(XDirection, _lastMoveDirection.x);
			_animator.SetFloat(Horizontal, movement.x);
			_animator.SetFloat(MovementAmount, Mathf.Clamp01(Mathf.Abs(movement.x) + Mathf.Abs(movement.y)));
		}

		#endregion
	}
}
