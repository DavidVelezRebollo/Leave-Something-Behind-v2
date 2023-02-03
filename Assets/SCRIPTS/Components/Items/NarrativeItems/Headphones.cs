using LSB.Components.Audio;

namespace LSB.Components.Items {
    public class Headphones : Item {

        public override void UseItem() { }

        public override void UndoItem() {
            SoundManager.Instance.Stop("ThemeSong");
        }
    }
}

