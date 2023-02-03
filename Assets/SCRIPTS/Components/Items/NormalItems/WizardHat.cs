using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items
{
    public class WizardHat : Item
    {
        [SerializeField] private Projectile WizardProjectileStats;
        private const float _DAMAGE_PERCENTAGE = 0.9f;

        public override void UseItem()
        {
            WizardProjectileStats.Damage *= _DAMAGE_PERCENTAGE;
        }

        public override void UndoItem()
        {
            WizardProjectileStats.Damage /= _DAMAGE_PERCENTAGE;
        }
    }
}