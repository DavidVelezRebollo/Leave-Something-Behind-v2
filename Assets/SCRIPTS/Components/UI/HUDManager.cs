using UnityEngine;
using LSB.Classes.UI;
using TMPro;

namespace LSB.Components.UI {
	public class HUDManager : MonoBehaviour {
		[SerializeField] private TextMeshProUGUI TimerText;
		
		private Timer _timer;
		
		private void Start() {
			_timer = new Timer(30f);
		}

		private void Update() {
			TimerText.text = $"{_timer.GetMinuteCount():00}:{_timer.GetSecondCount():00}";
			_timer.UpdateTimer();
		}
	}
	
}
