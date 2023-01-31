using UnityEngine;

namespace LSB.Interfaces {
    public interface IEnemyMove {
        /// <summary>
        /// Moves the enemy towards the player position
        /// </summary>
        /// <param name="playerPosition">The current position of the player</param>
        public void Move(Vector3 playerPosition);
        /// <summary>
        /// Gets the last direction where the enemy was looking
        /// </summary>
        /// <returns>A Vector2 which represents the last direction the enemy was looking</returns>
        public Vector2 GetLastDirection();
    }
}
