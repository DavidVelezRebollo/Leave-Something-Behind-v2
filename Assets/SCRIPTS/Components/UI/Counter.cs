using LSB.Components.Core;
using LSB.Shared;
using UnityEngine;
using TMPro;

namespace LSB.Components.UI {
    public class Counter : MonoBehaviour {
        #region Serialize Fields

        [SerializeField] private int CountTime;

        #endregion

        #region Private Fields

        private GameManager _gameManager;
        private TextMeshProUGUI _timerText;
        private float _timerDelta;

        #endregion

        #region Unity Events

        private void OnEnable() {
            if (!_timerText && !_gameManager) {
                _timerText = GetComponent<TextMeshProUGUI>();
                _gameManager = GameManager.Instance;
            }

            _timerDelta = CountTime;
        }

        private void Update() {
            if (_timerDelta <= 1) {
                gameObject.SetActive(false);
                return;
            }

            _timerDelta -= Time.deltaTime;
            _timerText.text = $"{_timerDelta:0}";
        }

        private void OnDisable() {
            _gameManager.SetGameState(GameState.Running);
        }

        #endregion
    }
}
