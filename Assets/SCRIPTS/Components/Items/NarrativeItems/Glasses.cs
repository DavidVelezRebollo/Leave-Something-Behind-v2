using UnityEngine;

namespace LSB.Components.Items {
    public class Glasses : Item {
        private Blur _blur;

        public override void UseItem() {
            _blur = Camera.main.GetComponent<Blur>();
            _blur.enabled = false;
        }

        public override void UndoItem() {
            _blur.enabled = true;
        }
    }
}

