using LSB.Interfaces;
using UnityEngine;

namespace LSB.Classes.Enemies {
    public class MeleeAttack : IAttack {
        private readonly EnemyAnimation _animation;
        private readonly Transform _playerTransform;
        private readonly Transform _transform;
        private readonly float _attackDistance;

        public MeleeAttack(Transform transform, float attackDistance, EnemyAnimation animation) {
            _transform = transform;
            _attackDistance = attackDistance;
            _animation = animation;
            
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void Attack() {
            
        }
    }
}

