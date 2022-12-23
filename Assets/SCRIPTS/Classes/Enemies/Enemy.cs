using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSB
{
    public abstract class Enemy
    {
        private IMove _movement;
        private IAttack _attack;
        public IState State;
        private Stats _attributes;

        private void takeDamage(float ammount) { }
    }
}
