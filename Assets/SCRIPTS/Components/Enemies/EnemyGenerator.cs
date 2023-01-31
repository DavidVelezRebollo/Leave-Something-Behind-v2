using System;
using System.Collections;
using LSB.Classes.ObjectPool;
using LSB.Components.Core;
using LSB.Components.Items;
using LSB.Components.Player;
using LSB.Components.UI;
using LSB.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LSB.Components.Enemies {
	public class EnemyGenerator : MonoBehaviour {
		[Header("Numeric Fields")]
		[SerializeField] private float EnemyGenerationCooldown;
		[Range(1, 100)] [SerializeField] private int[] MaxEnemies;
		[Header("Unity Prefabs")]
		[SerializeField] private MonoBehaviour OrcPrefab;
		[SerializeField] private MonoBehaviour WizardPrefab;

		private ObjectPool _orcPool;
		private ObjectPool _wizardPool;

		private Transform _playerTransform;
		private GameManager _gameManager;
		private HUDManager _hud;
		private float _generationDelta;
		private bool _canGenerate;		// dirtyFlag
		private bool _generating;

		private int _wizardNumber;
		private int _orcNumber;
		
		private int _currentWave;

		private void OnEnable() {
			resetStats();
	        _gameManager = GameManager.Instance;
	        _hud = FindObjectOfType<HUDManager>();
	        BackPack.Instance.OnItemInitialize += () => { _canGenerate = true; };

	        _orcPool = new ObjectPool((IPooledObject)OrcPrefab, true);
	        _wizardPool = new ObjectPool((IPooledObject)WizardPrefab, true);

			_generationDelta = EnemyGenerationCooldown;
			_canGenerate = false;
		}
		
		private void Start() {
			_playerTransform = FindObjectOfType<PlayerManager>().gameObject.transform;
		}

		private void Update() {
			if (_gameManager.GameEnded() || _gameManager.GamePaused()) return;
			
			_generationDelta -= Time.deltaTime;
			if (_generationDelta <= 0) _canGenerate = true;
			if (!_canGenerate || _generating) return;

			handleWaves();
		}

		private IEnumerator generateEnemies(GameObject enemyPrefab, Type enemyType, int numEnemies) {
			float x, y;
			int i = 0;
			bool follow = true;

			while (i < numEnemies && follow) {
				Vector3 playerPosition = _playerTransform.position;
				x = playerPosition.x + (Random.value < 0.5f ? -5f : 5f);
				y = playerPosition.y + Random.Range(-5f, 5f);
				_generating = true;

				GameObject enemy = Instantiate(enemyPrefab, new Vector3(x, y), Quaternion.identity);
				if (enemyType == typeof(Orc)) {
					enemy.GetComponent<Orc>().SubscribeEvent(onEnemyDieInvoke);
					_orcNumber++;
					if (_orcNumber >= MaxEnemies[0]) follow = false;
				}

				if (enemyType == typeof(Wizard)) {
					enemy.GetComponent<Wizard>().SubscribeEvent(onEnemyDieInvoke);
					_wizardNumber++;
					if (_wizardNumber >= MaxEnemies[1]) follow = false;
				}
				
				i++;

				yield return new WaitForSeconds(0.5f);
			}
			
			_canGenerate = false;
			_generating = false;
		}

		private void handleWaves() {
			if (_hud.GetMinutes() > 9f) {
				StartCoroutine(generateEnemies(OrcPrefab.gameObject, typeof(Orc), 10));
			} else if (_hud.GetMinutes() <= 9f && _hud.GetMinutes() > 7f) {
				StartCoroutine(generateEnemies(OrcPrefab.gameObject, typeof(Orc), 15));
				StartCoroutine(generateEnemies(WizardPrefab.gameObject, typeof(Wizard), 3));
			} else if (_hud.GetMinutes() <= 7f && _hud.GetMinutes() > 5f) {
				StartCoroutine(generateEnemies(OrcPrefab.gameObject, typeof(Orc), 8));
				StartCoroutine(generateEnemies(WizardPrefab.gameObject, typeof(Wizard), 5));
			} else if (_hud.GetMinutes() <= 5f && _hud.GetMinutes() > 3f) {
				StartCoroutine(generateEnemies(OrcPrefab.gameObject, typeof(Orc), 5));
				StartCoroutine(generateEnemies(WizardPrefab.gameObject, typeof(Wizard), 8));
			} else if (_hud.GetMinutes() <= 3f && _hud.GetMinutes() > 1f) {
				StartCoroutine(generateEnemies(OrcPrefab.gameObject, typeof(Orc), 3));
				StartCoroutine(generateEnemies(WizardPrefab.gameObject, typeof(Wizard), 15));
			} else if (_hud.GetMinutes() < 1f) {
				StartCoroutine(generateEnemies(WizardPrefab.gameObject, typeof(Wizard), 20));
			}
			
			_generationDelta = EnemyGenerationCooldown;
		}

		private void onEnemyDieInvoke(GameObject enemy) {
			if(enemy.GetComponent<Orc>())
				_orcNumber--;
			else if (enemy.GetComponent<Wizard>())
				_wizardNumber--;
		}

		private void resetStats()
        {
			OrcPrefab.GetComponent<Orc>().ResetStats();
			WizardPrefab.GetComponent<Wizard>().ResetStats();
		}
	}
}
