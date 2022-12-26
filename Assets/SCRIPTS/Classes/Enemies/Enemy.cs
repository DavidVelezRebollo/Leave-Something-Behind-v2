using LSB.Interfaces;
using LSB.Shared;
using UnityEngine;

namespace LSB.Classes.Enemies {
    public class Enemy
    {
        private readonly IEnemyMove _movement;
        private IAttack _attack;
        public IState State;
        private Stats _attributes;

        private Transform _player;

        public Enemy(IEnemyMove movementType, IAttack attackType, Stats attributes) {
            _movement = movementType;
            _attack = attackType;
            _attributes = attributes;
        }
        
        public void Start() {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public void TakeDamage(float amount) {
            
        }

        public void Move() {
            _movement?.Move(_player.position);
        }
    }
}
