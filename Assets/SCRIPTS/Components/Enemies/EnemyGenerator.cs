using System;
using LSB.Classes.Enemies;
using LSB.Components.Core;
using LSB.Components.Items;
using LSB.Components.Player;
using UnityEngine;
using Random = UnityEngine.Random;
using LSB.Shared;

namespace LSB.Components.Enemies {
	public class EnemyGenerator : MonoBehaviour {
		[Header("Numeric Fields")]
		[SerializeField] private float EnemyGenerationCooldown;
		[Range(1, 20)] [SerializeField] private int MaxEnemies;
		[Header("Unity Prefabs")]
		[SerializeField] private GameObject OrcPrefab;
		[SerializeField] private Stats OrcCurrentStats;
		[SerializeField] private Stats OrcBaseStats;
		[SerializeField] private GameObject WizardPrefab;

		private Transform _playerTransform;
		private GameManager _gameManager;
		private float _generationDelta;
		private bool _canGenerate;
		private int _waveNumber;
		private int _enemyNumber;
		private int _currentWave;

        private void Awake()
        {
			ResetStats();
        }

        private void OnEnable() {
	        _gameManager = GameManager.Instance;
	        BackPack.Instance.OnItemInitialize += () => { _canGenerate = true; };

			_generationDelta = EnemyGenerationCooldown;
			_canGenerate = false;
			_enemyNumber = 0;
			_waveNumber = 0;
		}
		
		private void Start() {
			_playerTransform = FindObjectOfType<PlayerManager>().gameObject.transform;
		}

		private void Update() {
			if (_gameManager.GameEnded() || _gameManager.GamePaused()) return;
			_generationDelta -= Time.deltaTime;
			if (_generationDelta <= 0) _canGenerate = true;

			if (!_canGenerate || _enemyNumber >= MaxEnemies) return;

			generateWave(OrcPrefab, typeof(Orc), 2);
			_canGenerate = false;
			_generationDelta = EnemyGenerationCooldown;
		}

		private void generateWave(GameObject enemyPrefab, Type enemyType, int numEnemies) {
			float x, y;
			int i = 0;
			bool follow = true;

			while (i < numEnemies && follow) {
				Vector3 playerPosition = _playerTransform.position;
				x = playerPosition.x + (Random.value < 0.5f ? -5f : 5f);
				y = playerPosition.y + Random.Range(-5f, 5f);

				GameObject enemy = Instantiate(enemyPrefab, new Vector3(x, y), Quaternion.identity);
				if(enemyType == typeof(Orc)) enemy.GetComponent<Orc>().SuscribeEvent(onEnemyDieInvoke);

				_enemyNumber++;
				i++;

				if (_enemyNumber == MaxEnemies) follow = false;
			}
		}

		private void onEnemyDieInvoke() {
			_enemyNumber--;
		}

		private void ResetStats()
		{
			OrcCurrentStats.Damage = OrcBaseStats.Damage;
			OrcCurrentStats.Speed = OrcBaseStats.Speed;
			OrcCurrentStats.MaxHp = OrcBaseStats.MaxHp;
		}
	}
}
