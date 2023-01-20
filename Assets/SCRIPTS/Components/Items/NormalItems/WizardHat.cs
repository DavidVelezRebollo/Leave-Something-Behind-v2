using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items
{
    public class WizardHat : Item
    {
        [SerializeField] private Stats WizardStats;
        private const float _DAMAGE_PERCENTAGE = 1 - 0.10f;

        public override void UseItem()
        {
            WizardStats.Damage *= _DAMAGE_PERCENTAGE;
        }

        public override void UndoItem()
        {
            WizardStats.Damage /= _DAMAGE_PERCENTAGE;
        }
    }
}