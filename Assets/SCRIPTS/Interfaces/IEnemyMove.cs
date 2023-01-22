using UnityEngine;

namespace LSB.Interfaces {
    public interface IEnemyMove {
        public void Move(Vector3 playerPosition);
        public Vector2 GetLastDirection();
    }
}
