using LSB.Classes.Enemies;
using LSB.Interfaces;
using UnityEngine;

namespace LSB.Classes.State {
    public class Attacking : IState {
        private readonly Enemy _enemy;
        private readonly float _stopDistance;
        private readonly Transform _enemyTransform;
        private Transform _player;

        public Attacking(Enemy enemy, Transform transform, float stopDistance) {
            _enemy = enemy;
            _stopDistance = stopDistance;
            _enemyTransform = transform;
        }
        
        public void Enter() {
            _player = GameObject.FindWithTag("Player").transform;
        }

        public void Exit() { }

        public void FixedUpdate() {
            float distance = Vector3.Distance(_player.position, _enemyTransform.position);
            
            if(distance <= _stopDistance)
                _enemy.Attack();
            else
                _enemy.SetState(new Chasing(_enemy, _enemyTransform, _stopDistance));
        }
    }
}
