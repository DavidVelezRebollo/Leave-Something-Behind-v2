using UnityEngine;

namespace LSB.Classes.UI {
	public class Timer {
		private float _timer;
		private readonly int _totalMinutes;
		private int _currentMinutes;
		private float _currentSeconds;

		public Timer(int totalMinutes) {
			_totalMinutes = totalMinutes * 60;
			_timer = totalMinutes * 60;
			_currentMinutes = totalMinutes * 60;
			_currentSeconds = 0f;
		}

		public void UpdateTimer() {
			_timer -= Time.deltaTime;
			_currentMinutes = Mathf.FloorToInt(_timer / 60f);
			_currentSeconds = Mathf.FloorToInt(_timer % 60f);
		}

		public int GetMinuteCount() { return _currentMinutes; }
		
		public float GetSecondCount() { return _currentSeconds; }

		public int GetTotalMinutes() { return _totalMinutes; }
	}
}
