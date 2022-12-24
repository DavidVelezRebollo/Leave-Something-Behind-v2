using LSB.Shared;

namespace LSB.Classes.Items {
    public abstract class Item {
        private ItemInfo _item;

        public virtual void UseItem() {
            
        }
        public void DeleteItem() 
        { 
            //TODO Implent DeleteItem
        }
        public void AddToBackPack() 
        { 
            //TODO Implement AddToBackPack
        }
    }
}

