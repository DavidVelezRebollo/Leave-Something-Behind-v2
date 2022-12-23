using UnityEngine;
using UnityEngine.Audio;
using LSB.Classes.Sound;

namespace LSB.Components.Audio {
	public class SoundManager : MonoBehaviour {
		public static SoundManager Instance;

		[SerializeField] private AudioMixer GeneralMixer;
		[SerializeField] private AudioMixer MusicMixer;
		[SerializeField] private AudioMixer SoundEffectsMixer;
		[SerializeField] private Sound[] Sounds;

		private void Awake() {
			// TODO Initialize sounds
		}

		private void Start() {
			// TODO Initialize Audio Mixers
		}

		private void LoadVolume() {
			// TODO Load volume
		}

		private void Play() {
			// TODO Play
		}

		private void PlayOneShot() {
			// TODO PlayOneShot
		}
		
		private void Stop() {
            // TODO Stop
        }
	}
}
