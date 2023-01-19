using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items
{
    public class ConstructionHelmet : Item
    {
        private float damagePercentage = 1 - 0.10f;
        [SerializeField] private Stats OrcStats;

        public override void UseItem()
        {
            OrcStats.Damage *= damagePercentage;
        }

        public override void UndoItem()
        {
            OrcStats.Damage /= damagePercentage;
        }
    }
}