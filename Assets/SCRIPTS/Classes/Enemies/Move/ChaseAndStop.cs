using LSB.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace LSB.Classes.Enemies {
    public class ChaseAndStop : IEnemyMove {
        private readonly NavMeshAgent _agent;
        private Vector3 _playerPosition;
        
        public ChaseAndStop(NavMeshAgent agent, float speed , float stopDistance) {
            _agent = agent;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.stoppingDistance = stopDistance;

            _agent.speed = speed;
        }
        
        public void Move(Vector3 playerPosition) {
            _agent.Move(_playerPosition);
        }

        public void Update(Vector3 playerPosition) {
            _playerPosition = playerPosition;
            Move(_playerPosition);
        }

        public bool IsStopped() {
            return _agent.isStopped;
        }
    }
}

