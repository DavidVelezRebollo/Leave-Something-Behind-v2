using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Shared;
using LSB.Components.Core;

namespace LSB.Components.Items
{
    public class Android : Item
    {
        private const float _CORRUPTION_MODIFIER = 1f;

        public override void UseItem()
        {
            GameManager.Instance.AddCorruptionDamage(-_CORRUPTION_MODIFIER);
        }

        public override void UndoItem() {
            GameManager.Instance.AddCorruptionDamage(_CORRUPTION_MODIFIER);
        }
    }
}
