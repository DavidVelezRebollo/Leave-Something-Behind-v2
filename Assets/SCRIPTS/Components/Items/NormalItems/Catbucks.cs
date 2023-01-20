using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items
{
    public class Catbucks : Item
    {
        [SerializeField] private Stats PlayerStats;
        private const float _LIVE_PERCENTAGE = 1 + 0.10f;

        public override void UseItem()
        {
            PlayerStats.MaxHp *= _LIVE_PERCENTAGE;
        }

        public override void UndoItem()
        {
            PlayerStats.MaxHp /= _LIVE_PERCENTAGE;
        }
    }
}