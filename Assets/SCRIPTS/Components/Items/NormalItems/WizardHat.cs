using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items
{
    public class WizardHat : Item
    {
        private float damagePercentage = 1 - 0.10f;
        [SerializeField] private Stats WizardStats;

        public override void UseItem()
        {
            WizardStats.Damage *= damagePercentage;
        }

        public override void UndoItem()
        {
            WizardStats.Damage /= damagePercentage;
        }
    }
}