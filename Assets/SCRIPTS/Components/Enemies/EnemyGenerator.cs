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
		[Header("Enemies Prefabs")]
		[SerializeField] private MonoBehaviour OrcPrefab;
		[SerializeField] private MonoBehaviour WizardPrefab;
		[SerializeField] private MonoBehaviour GoblinPrefab;

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
		private int _goblinNumber;

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
			if (_gameManager.GamePaused()) return;
			
			_generationDelta -= Time.deltaTime;
			if (_generationDelta <= 0) _canGenerate = true;
			if (!_canGenerate || _generating) return;

			handleWaves();
		}

		private IEnumerator generateEnemies(GameObject enemyPrefab, Type enemyType, int numEnemies, float respawnTime) {
			float x, y;
			int i = 0;
			bool follow = true;

			while (i < numEnemies && follow) {
				Vector3 playerPosition = _playerTransform.position;
				if(Random.value < 0.5f) {
					x = playerPosition.x + Random.Range(-7f, 7f);
					y = playerPosition.y + (Random.value < 0.5f ? -7f : 7f);
				} else {
					x = playerPosition.x + (Random.value < 0.5f ? -7f : 7f);
					y = playerPosition.y + Random.Range(-7f, 7f);
				}
				
				_generating = true;

				do {
					yield return null;
				} while (_gameManager.GamePaused());

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

				if (enemyType == typeof(Goblin)) {
					enemy.GetComponent<Goblin>().SubscribeEvent(onEnemyDieInvoke);
					_goblinNumber++;
					if (_goblinNumber >= MaxEnemies[2]) follow = false;
				}
				
				i++;

				yield return new WaitForSeconds(respawnTime);
			}
			
			_canGenerate = false;
			_generating = false;
		}

		private void handleWaves() {
			if (_hud.GetMinutes() > 9f) {
				StartCoroutine(generateEnemies(OrcPrefab.gameObject, typeof(Orc), 20, 0.2f));
			} else if (_hud.GetMinutes() <= 9f && _hud.GetMinutes() > 7f) {
				StartCoroutine(generateEnemies(OrcPrefab.gameObject, typeof(Orc), 15, 0.2f));
				StartCoroutine(generateEnemies(GoblinPrefab.gameObject, typeof(Goblin), 4, 0.8f));
			} else if (_hud.GetMinutes() <= 7f && _hud.GetMinutes() > 5f) {
				StartCoroutine(generateEnemies(GoblinPrefab.gameObject, typeof(Goblin), 6, 0.4f));
				StartCoroutine(generateEnemies(WizardPrefab.gameObject, typeof(Wizard), 10, 0.8f));
			} else if (_hud.GetMinutes() <= 5f && _hud.GetMinutes() > 3f) {
				StartCoroutine(generateEnemies(OrcPrefab.gameObject, typeof(Orc), 15, 0.2f));
				StartCoroutine(generateEnemies(WizardPrefab.gameObject, typeof(Wizard), 7, 0.8f));
			} else if (_hud.GetMinutes() <= 3f && _hud.GetMinutes() > 1f) {
				StartCoroutine(generateEnemies(OrcPrefab.gameObject, typeof(Orc), 10, 0.2f));
				StartCoroutine(generateEnemies(WizardPrefab.gameObject, typeof(Wizard), 6, 0.8f));
				StartCoroutine(generateEnemies(GoblinPrefab.gameObject, typeof(Goblin), 5, 0.4f));
			} else if (_hud.GetMinutes() <= 1f) {
				StartCoroutine(generateEnemies(OrcPrefab.gameObject, typeof(Orc), 15, 0.3f));
				StartCoroutine(generateEnemies(WizardPrefab.gameObject, typeof(Wizard), 6, 0.8f));
				StartCoroutine(generateEnemies(GoblinPrefab.gameObject, typeof(Goblin), 8, 0.4f));
			}
			
			_generationDelta = EnemyGenerationCooldown;
		}

		private void onEnemyDieInvoke(GameObject enemy) {
			if(enemy.GetComponent<Orc>())
				_orcNumber--;
			else if (enemy.GetComponent<Wizard>())
				_wizardNumber--;
			else if (enemy.GetComponent<Goblin>())
				_goblinNumber--;
		}

		private void resetStats()
        {
			OrcPrefab.GetComponent<Orc>().ResetStats();
			WizardPrefab.GetComponent<Wizard>().ResetStats();
			GoblinPrefab.GetComponent<Goblin>().ResetStats();
		}
	}
}
