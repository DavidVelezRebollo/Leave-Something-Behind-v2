using UnityEngine;
using LSB.Shared;

namespace LSB.Components.Items
{
    public class WizardHat : Item
    {
        [SerializeField] private Stats WizardCurrentStats;
        private const float _DAMAGE_PERCENTAGE = 0.9f;

        public override void UseItem() { WizardCurrentStats.Damage *= _DAMAGE_PERCENTAGE; }

        public override void UndoItem() { WizardCurrentStats.Damage /= _DAMAGE_PERCENTAGE; }
    }
}