using UnityEngine;

namespace LSB.Classes.UI {
	public class Timer {
		private float _timer;
		private float _currentMinutes;
		private float _currentSeconds;

		public Timer(float minutes) {
			_currentMinutes = minutes;
			_currentSeconds = 0f;
		}

		public void UpdateTimer() {
			_timer += Time.deltaTime;
			_currentMinutes = Mathf.FloorToInt(_timer / 60f);
			_currentSeconds = Mathf.FloorToInt(_timer % 60f);
		}

		public float GetMinuteCount() { return _currentMinutes; }
		
		public float GetSecondCount() { return _currentSeconds; }
	}
}
