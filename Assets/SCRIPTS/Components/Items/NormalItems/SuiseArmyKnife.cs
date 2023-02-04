using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items
{
    public class SuiseArmyKnife : Item
    {
        [SerializeField] private Stats PlayerStats;
        private const float _DAMAGE_PERCENTAGE = 1.1f;

        public override void UseItem() { PlayerStats.Damage *= _DAMAGE_PERCENTAGE; }

        public override void UndoItem() { PlayerStats.Damage /= _DAMAGE_PERCENTAGE; }
    }
}

