using LSB.Components.Items;
using LSB.Components.Player;
using UnityEngine;

namespace LSB.Components.Enemies {
	public class EnemyGenerator : MonoBehaviour {
		[SerializeField] private float EnemyGenerationCooldown;
		[SerializeField] private GameObject OrcPrefab;
		[SerializeField] private GameObject WizardPrefab;

		private int _waveNumber;
		private float _generationDelta;
		private bool _canGenerate;
		private Vector3 _playerPosition;

		private void OnEnable() {
			BackPack.Instance.OnItemInitialize += () => { _canGenerate = true; };

			_generationDelta = EnemyGenerationCooldown;
			_canGenerate = false;
		}

		private void Update() {
			_generationDelta -= Time.deltaTime;
			if (_generationDelta <= 0) _canGenerate = true;

			if (!_canGenerate) return;

			generateWave(OrcPrefab, 2);
			_canGenerate = false;
			_generationDelta = EnemyGenerationCooldown;
		}

		private void generateWave(GameObject enemyPrefab, int numEnemies) {
			float x, y;

			for (int i = 0; i < numEnemies; i++) {
				_playerPosition = FindObjectOfType<PlayerManager>().gameObject.transform.position;
				x = _playerPosition.x + (Random.value < 0.5f ? -5f : 5f);
				y = _playerPosition.y + Random.Range(-5f, 5f);

				GameObject enemy = Instantiate(enemyPrefab, new Vector3(x, y), Quaternion.identity);
			}
		}
	}
}
