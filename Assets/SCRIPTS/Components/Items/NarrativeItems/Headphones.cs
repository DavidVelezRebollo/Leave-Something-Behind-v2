using LSB.Components.Audio;

namespace LSB.Components.Items {
    public class Headphones : Item {

        public override void UseItem() { SoundManager.Instance.SetMusicActive(true); }

        public override void UndoItem() {
            SoundManager.Instance.SetMusicVolume(-50);
            SoundManager.Instance.SetMusicActive(false);
        }
    }
}

