using LSB.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace LSB.Classes.Enemies {
    public class Chase : IEnemyMove {
        private readonly NavMeshAgent _agent;
        private Vector3 _playerPosition;
        
        public Chase(NavMeshAgent agent, float speed) {
            _agent = agent;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.stoppingDistance = 0.5f;

            _agent.speed = speed;
        }
        
        public void Move(Vector3 playerPosition) {
            _playerPosition = playerPosition;
            
            _agent.SetDestination(_playerPosition);
        }
    }
}