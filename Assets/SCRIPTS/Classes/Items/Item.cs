using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSB
{
    public abstract class Item : MonoBehaviour
    {
        private ItemInfo _item;

        public virtual void UseItem() { }
        public void DeleteItem() 
        { 
            //TODO-implent
        }
        public void AddToBackPack() 
        { 
            //TODO-implement
        }
    }
}

