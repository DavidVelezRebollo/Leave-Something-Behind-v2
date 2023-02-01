using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSB.Classes.Player;

namespace LSB.Components.Items
{
    public class WaterGun : Item
    {
        [SerializeField] private PlayerAttack playerAttack;
        [SerializeField] private GameObject proyectilePrefab;
        [SerializeField] private GameObject defaultProyectilePrefab;

        public override void UseItem()
        {
            playerAttack.SetProyectilePrefab(proyectilePrefab);
        }

        public override void UndoItem()
        {
            playerAttack.SetProyectilePrefab(defaultProyectilePrefab);
        }
    }
}
