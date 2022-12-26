using LSB.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace LSB.Classes.Enemies {
    public class ChaseAndStop : IEnemyMove {
        private readonly NavMeshAgent _agent;
        private readonly float _stopDistance;
        private Vector3 _playerPosition;
        private Vector3 _enemyPosition;

        public ChaseAndStop(NavMeshAgent agent, float speed , float stopDistance) {
            _agent = agent;

            _agent.speed = speed;
            _stopDistance = stopDistance;
        }
        
        public void Move(Vector3 playerPosition) {
            if (Vector3.Distance(_enemyPosition, _playerPosition) <= _stopDistance) {
                _agent.ResetPath();
                return;
            }
            
            _agent.Move(_playerPosition);
        }

        public void Update(Vector3 playerPosition, Vector3 enemyPosition) {
            _playerPosition = playerPosition;
            _enemyPosition = enemyPosition;
            Move(_playerPosition);
        }

        public bool IsStopped() {
            return _agent.isStopped;
        }
    }
}

