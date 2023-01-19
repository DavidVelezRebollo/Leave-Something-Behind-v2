using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items
{
    public class Catbucks : Item
    {
        [SerializeField] private Stats PlayerStats;
        private float livePercentage = 1 + 0.10f;
        public override void UseItem()
        {
            PlayerStats.MaxHp *= livePercentage;
        }

        public override void UndoItem()
        {
            PlayerStats.MaxHp /= livePercentage;
        }
    }
}