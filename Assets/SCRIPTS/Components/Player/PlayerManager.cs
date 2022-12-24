using UnityEngine;
using LSB.Classes.Items;
using LSB.Shared;

namespace LSB.Components.Player {
	public class PlayerManager : MonoBehaviour {
		private Stats _attributes;
		private IShoot _shoot;
		private IMove _movement;
		private BackPack _backPack;

		private void Start() {
			
		}

		private void takeDamage(float ammount) {
			// TODO Player takes damage
		}
	}
}
