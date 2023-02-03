using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Classes.Player;

namespace LSB.Components.Items
{
    public class WaterGun : Item
    {
        private PlayerAttack _playerAttack;
        [SerializeField] private GameObject ProjectilePrefab;

        public override void UseItem() {
            _playerAttack = FindObjectOfType<PlayerAttack>();
            _playerAttack.AddProjectilePrefabs(ProjectilePrefab);
        }

        public override void UndoItem() { _playerAttack.RemoveProjectilePrefabs(ProjectilePrefab); }
    }
}
