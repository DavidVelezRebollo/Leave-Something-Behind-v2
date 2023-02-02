using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Classes.Player;

namespace LSB.Components.Items
{
    public class WaterGun : Item
    {
        private PlayerAttack _playerAttack;
        [SerializeField] private GameObject proyectilePrefab;
        [SerializeField] private GameObject defaultProyectilePrefab;

        private void Start() {
            _playerAttack = FindObjectOfType<PlayerAttack>();
        }
        public override void UseItem()
        {
            _playerAttack.GetProyectilePrefab(proyectilePrefab);
        }

        public override void UndoItem()
        {
            _playerAttack.GetProyectilePrefab(defaultProyectilePrefab);
        }
    }
}
