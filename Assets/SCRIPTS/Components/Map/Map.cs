using System.Collections;
using System.Collections.Generic;
using LSB.Components.UI;
using LSB.Shared;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace LSB.Components.Core {
	public class Map : MonoBehaviour {
		[Header("Tilemaps")]
		[SerializeField] private Tilemap FloorTilemap;
		[SerializeField] private Tilemap DecorationTilemap;
		[SerializeField] private Tilemap CorruptionTilemap;
		[SerializeField] private Tilemap CorruptionDecorationTilemap;

		[Space(10)] [Header("Tiles")] 
		[SerializeField] private TileBase Grass;
		[SerializeField] private TileBase[] Decoration;
		[SerializeField] private TileBase[] Tree;
		[SerializeField] private TileBase[] Lamp;
		[SerializeField] private TileBase[] Tower;
		[SerializeField] private TileBase Car;
		[SerializeField] private TileBase Hen;

		[Space(10)] [Header("Corrupted Tiles")] 
		[SerializeField] private TileBase CorruptedGrass;
		[SerializeField] private TileBase CorruptedGrassGlitched;
		[SerializeField] private TileBase[] CorruptedDecoration;
		[SerializeField] private TileBase[] CorruptedTree;
		[SerializeField] private TileBase[] CorruptedLamp;
		[SerializeField] private TileBase[] CorruptedTower;
		[SerializeField] private TileBase CorruptedCar;
		[SerializeField] private TileBase CorruptedHen;

		[Space(10)] [Header("Map variables")] 
		[SerializeField] private int MapHeight = 50;
		[SerializeField] private int MapWidth = 50;

		[Header("Unity Fields")] 
		[SerializeField] private GameObject PlayerPrefab;

		private Transform _player;
		private Vector3Int _playerCellPosition;

		private int _currentGridHeightUp;
		private int _currentGridHeightDown;
		private int _currentGridWidthRight;
		private int _currentGridWidthLeft;

		private bool _corrupted;
		private bool _generating;
		private int _lastSecond;
		private List<Vector3Int> _expansionTiles;
		private HUDManager _hudTimer;

		private const float _LAMP_PROBABILITY = 0.01f;
		private const float _TOWER_PROBABILITY = 0.04f;
		private const float _CAR_PROBABILITY = 0.05f;
		private const float _HEN_PROBABILITY = 0.1f;
		private const float _TREES_PROBABILITY = 0.15f;
		private const float _DECORATION_PROBABILITY = 0.35f;
		
		private Props[,] _cellMap;

		private void Awake() {
			_cellMap = new Props[MapWidth, MapHeight];

			_currentGridHeightDown = _currentGridHeightUp = MapHeight / 2;
			_currentGridWidthLeft = _currentGridWidthRight = MapWidth / 2;
			_expansionTiles = new List<Vector3Int>();

			generateCellMap();
			drawMap();
		}

		private void Start() {
			_hudTimer = FindObjectOfType<HUDManager>();
		}

		private void Update() {
			_playerCellPosition = FloorTilemap.WorldToCell(_player.position);
			infiniteGeneration();

			if (_lastSecond != _hudTimer.GetSeconds())
				_corrupted = false;
			
			if (_generating == false)
				StartCoroutine(CorruptTerrain());

			if (_hudTimer.GetSeconds() % 59 != 0 || _hudTimer.GetMinutes() > 9f || _corrupted || _hudTimer.GetSeconds() == 0) return;
			
			NewExpansionTile();
			_corrupted = true;
			_lastSecond = _hudTimer.GetSeconds();
		}

		private void generateCellMap() {
			float[,] noiseMap = generatePerlinNoise();
			
			for(int y = 0; y < MapHeight; y++)
				for (int x = 0; x < MapHeight; x++) {
					float value = noiseMap[x, y];
					
					if(_cellMap[x, y] != Props.None) continue;

					if (value > _TREES_PROBABILITY && value <= _DECORATION_PROBABILITY) // DECORATION
						_cellMap[x, y] = Props.Decoration;
					else if (value > _HEN_PROBABILITY && value <= _TREES_PROBABILITY) { // TREES
						if (y == MapHeight - 1 || y == 0) {
							_cellMap[x, y] = Props.Grass;
							continue;
						}
						
						if (_cellMap[x, y + 1] == Props.Tree || _cellMap[x, y - 1] == Props.Tree) {
							_cellMap[x, y] = Props.Grass;
							continue;
						}
						
						_cellMap[x, y] = Props.Tree;
						_cellMap[x, y + 1] = Props.Tree;
					}
					else if (value <= _LAMP_PROBABILITY) { // LAMPS
						if (y == MapHeight - 1 || y == 0) {
							_cellMap[x, y] = Props.Grass;
							continue;
						}
						
						if (_cellMap[x, y + 1] == Props.Lamp || _cellMap[x, y - 1] == Props.Lamp) {
							_cellMap[x, y] = Props.Grass;
							continue;
						}
						
						_cellMap[x, y] = Props.Lamp;
						_cellMap[x, y + 1] = Props.Lamp;
					}
					else if (value > _LAMP_PROBABILITY && value <= _TOWER_PROBABILITY) { // TOWERS
						if (y == MapHeight - 1 || y == 0) {
							_cellMap[x, y] = Props.Grass;
							continue;
						}
						
						if (_cellMap[x, y + 1] == Props.Tower || _cellMap[x, y - 1] == Props.Tower) {
							_cellMap[x, y] = Props.Grass;
							continue;
						}
						
						_cellMap[x, y] = Props.Tower;
						_cellMap[x, y + 1] = Props.Tower;
					} 
					else if (value > _TOWER_PROBABILITY && value <= _CAR_PROBABILITY) // CARS
						_cellMap[x, y] = Props.Car;
					else if (value > _CAR_PROBABILITY && value <= _HEN_PROBABILITY) // HENS
						_cellMap[x, y] = Props.Hen;
					
					if(_cellMap[x, y] == Props.None) _cellMap[x, y] = Props.Grass;
				}
		}

		private void drawMap() {
			for(int y = 0; y < MapHeight; y++)
				for (int x = 0; x < MapWidth; x++) {
					bool isDecoration = false, isFloor = false;
					Vector3Int pos = new Vector3Int(MapWidth / 2 - x, MapHeight / 2 - y, 0);
					TileBase tile = null;

					switch (_cellMap[x, y]) {
						case Props.Tiled:
							continue;
						case Props.Grass:
							tile = Grass;
							isFloor = true;
							break;
						case Props.Decoration:
							int index = Random.Range(0, Decoration.Length);
							isFloor = true;
							tile = Decoration[index];
							break;
						case Props.Tree:
							if (y == 0) continue;
							if (_cellMap[x, y - 1] == Props.Tree) {
								tile = Tree[0];
								isDecoration = true;
								DecorationTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Tree[1]);
								FloorTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Grass);
								_cellMap[x, y - 1] = Props.Tiled;
							}

							break;
						case Props.Lamp:
							if (y == 0) continue;
							if (_cellMap[x, y - 1] == Props.Lamp) {
								tile = Lamp[0];
								isDecoration = true;
								DecorationTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Lamp[1]);
								FloorTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Grass);
								_cellMap[x, y - 1] = Props.Tiled;
							}
							
							break;
						case Props.Tower:
							if (y == 0) continue;
							if (_cellMap[x, y - 1] == Props.Tower) {
								tile = Tower[0];
								isDecoration = true;
								DecorationTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Tower[1]);
								FloorTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Grass);
								_cellMap[x, y - 1] = Props.Tiled;
							}
							
							break;
						case Props.Car:
							isDecoration = true;
							tile = Car;
							break;
						case Props.Hen:
							isDecoration = true;
							tile = Hen;
							break;
					}
					
					if(isFloor) FloorTilemap.SetTile(pos, tile);
					if (isDecoration) {
						FloorTilemap.SetTile(pos, Grass);
						DecorationTilemap.SetTile(pos, tile);
					}
				}
			
			spawnPlayer();
		}

		private void infiniteGeneration() {
			Vector3Int pos;
			Vector3Int delete;
			
			#region UPPER MAP
			
			if (_playerCellPosition.y >= _currentGridHeightUp - 8) {
				for (int y = 1; y < 3; y++)
					for (int x = -_currentGridWidthLeft + 1; x <= _currentGridWidthRight + 1; x++) {
						pos = new Vector3Int(x, _currentGridHeightUp + y, 0);
						delete = new Vector3Int(x, -_currentGridHeightDown + y, 0);
						
						generateTerrain(pos, delete);
					}
				_currentGridHeightUp += 2;
				_currentGridHeightDown -= 2;
			}

			#endregion

			#region LOWER MAP
			
			if (_playerCellPosition.y <= -_currentGridHeightDown + 8) {
				for (int y = 0; y < 2; y++)
					for (int x = -_currentGridWidthLeft + 1; x <= _currentGridWidthRight; x++) {
						pos = new Vector3Int(x, -_currentGridHeightDown - y, 0);
						delete = new Vector3Int(x, _currentGridHeightUp + y, 0);
						
						generateTerrain(pos, delete);
					}
				_currentGridHeightDown += 2;
				_currentGridHeightUp -= 2;
			}
				
			#endregion

			#region RIGHT MAP

			if (_playerCellPosition.x >= _currentGridWidthRight - 8) {
				for (int y = -_currentGridHeightDown + 1; y <= _currentGridHeightUp; y++)
					for (int x = 1; x < 3; x++) {
						pos = new Vector3Int(_currentGridWidthRight + x, y, 0);
						delete = new Vector3Int(-_currentGridWidthLeft + x, y, 0);
						
						generateTerrain(pos, delete);
					}
				_currentGridWidthRight += 2;
				_currentGridWidthLeft -= 2;
			}

			#endregion

			#region LEFT MAP

			if (_playerCellPosition.x <= -_currentGridWidthLeft + 8) {
				for (int y = -_currentGridHeightDown + 1; y <= _currentGridHeightUp; y++)
					for (int x = 0; x < 2; x++) {
						pos = new Vector3Int(-_currentGridWidthLeft - x, y, 0);
						delete = new Vector3Int(_currentGridWidthRight + x, y, 0);
						
						generateTerrain(pos, delete);
					}
				_currentGridWidthLeft += 2;
				_currentGridWidthRight -= 2;
			}

			#endregion
		}

		private float[,] generatePerlinNoise() {
			float[,] noiseMap = new float[MapWidth, MapHeight];
			float xOffset = Random.Range(-100000f, 100000f);
			float yOffset = Random.Range(-100000f, 100000f);
			const float SCALE = 0.5f;

			for(int x = 0; x < MapWidth; x++)
				for (int y = 0; y < MapHeight; y++) {
					float noise = Mathf.PerlinNoise(x * SCALE + xOffset, y * SCALE + yOffset);

					noiseMap[x, y] = noise;
				}

			return noiseMap;
		}

		private void generateTerrain(Vector3Int pos, Vector3Int deletePos) {
			FloorTilemap.SetTile(pos, Grass);

			if (DecorationTilemap.GetTile(pos) != null) {
				FloorTilemap.SetTile(deletePos, null);
				DecorationTilemap.SetTile(deletePos, null);
				return;
			}

			if (DecorationTilemap.GetTile(pos + new Vector3Int(0, 1, 0)) != null) {
				FloorTilemap.SetTile(deletePos, null);
				DecorationTilemap.SetTile(deletePos, null);
				return;
			}

			TileBase tile = null;
			bool isFloor = false, isDecoration = false;
			float[,] noiseMap = generatePerlinNoise();
			
			float value = noiseMap[Random.Range(0, MapWidth), Random.Range(0, MapHeight)];
			
			switch (value) {
				case > _TREES_PROBABILITY and <= _DECORATION_PROBABILITY: 
					int index = Random.Range(0, Decoration.Length);
					isFloor = true;
					tile = Decoration[index];
					break;
				case > _HEN_PROBABILITY and <= _TREES_PROBABILITY:
					isDecoration = true;
					tile = Tree[0];
					DecorationTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Tree[1]);
					break;
				case <= _LAMP_PROBABILITY:
					isDecoration = true;
					tile = Lamp[0];
					DecorationTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Lamp[1]);
					break;
				case > _LAMP_PROBABILITY and <= _TOWER_PROBABILITY:
					isDecoration = true;
					tile = Tower[0];
					DecorationTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Tower[1]);
					break;
				case > _TOWER_PROBABILITY and <= _CAR_PROBABILITY:
					isDecoration = true;
					tile = Car;
					break;
				case > _CAR_PROBABILITY and <= _HEN_PROBABILITY:
					isDecoration = true;
					tile = Hen;
					break;
			}
			
			if(isFloor) FloorTilemap.SetTile(pos, tile);
			else if (isDecoration) {
				FloorTilemap.SetTile(pos, Grass);
				DecorationTilemap.SetTile(pos, tile);
			}
			
			FloorTilemap.SetTile(deletePos, null);
			DecorationTilemap.SetTile(deletePos, null);
		}

		private void spawnPlayer() {
			// TODO - Determine if the player can spawn
			GameObject player = Instantiate(PlayerPrefab, new Vector3(0, 0), Quaternion.identity);
			_player = player.transform;
		}

		private void NewExpansionTile() {
			int x;
			int y;

			do {
				x = Random.Range(-4, 5);
				y = Random.Range(-4, 5);
			} while ((x > -2 && x < 2) || (y > -2 && y < 2));

			_expansionTiles.Add(new Vector3Int(_playerCellPosition.x + x, _playerCellPosition.y + y, 0));
		}

		private IEnumerator CorruptTerrain() {
			_generating = true;
			int initCount = _expansionTiles.Count;

			for (int i = 0; i < initCount; i++) {
				int x = Random.Range(-1, 2);
				int y = 0;

				if (x == 0) y = Random.Range(-1, 2);
				else {
					do y = Random.Range(-1, 2);
					while (y == 0);
				}

				do {
					yield return null;
				} while (GameManager.Instance.GamePaused());

				Vector3Int tile = new Vector3Int(_expansionTiles[i].x + x, _expansionTiles[i].y + y, 0);
				if (FloorTilemap.GetTile(tile) != Grass) {
					bool found = false;
					int a = 0;

					while (a < Decoration.Length && !found) {
						if (Decoration[a] == FloorTilemap.GetTile(tile)) {
							CorruptionTilemap.SetTile(tile, CorruptedDecoration[a]);
							found = true;
						}

						a++;
					}
				}
				else {
					CorruptionTilemap.SetTile(tile, Random.Range(0f, 1f) > 0.3f ? CorruptedGrass : CorruptedGrassGlitched);
				}

				if (DecorationTilemap.GetTile(tile)) {
					if (DecorationTilemap.GetTile(tile) == Tower[0]) {
						CorruptionDecorationTilemap.SetTile(tile, CorruptedTower[0]);
						CorruptionDecorationTilemap.SetTile(tile + new Vector3Int(0, 1, 0), CorruptedTower[1]);
						DecorationTilemap.SetTile(tile + new Vector3Int(0, 1, 0), null);
					}
					
					else if (DecorationTilemap.GetTile(tile) == Tower[1]) {
						CorruptionDecorationTilemap.SetTile(tile, CorruptedTower[1]);
						CorruptionDecorationTilemap.SetTile(tile + new Vector3Int(0, -1, 0), CorruptedTower[0]);
						DecorationTilemap.SetTile(tile + new Vector3Int(0, -1, 0), null);
					}

					else if (DecorationTilemap.GetTile(tile) == Tree[0]) {
						CorruptionDecorationTilemap.SetTile(tile, CorruptedTree[0]);
						CorruptionDecorationTilemap.SetTile(tile + new Vector3Int(0, 1, 0), CorruptedTree[1]);
						DecorationTilemap.SetTile(tile + new Vector3Int(0, 1, 0), null);
					}
					
					else if (DecorationTilemap.GetTile(tile) == Tree[1]) {
						CorruptionDecorationTilemap.SetTile(tile, CorruptedTree[1]);
						CorruptionDecorationTilemap.SetTile(tile + new Vector3Int(0, -1, 0), CorruptedTree[0]);
						DecorationTilemap.SetTile(tile + new Vector3Int(0, -1, 0), null);
					}
					
					else if (DecorationTilemap.GetTile(tile) == Lamp[0]) {
						CorruptionDecorationTilemap.SetTile(tile, CorruptedLamp[0]);
						CorruptionDecorationTilemap.SetTile(tile + new Vector3Int(0, 1, 0), CorruptedLamp[1]);
						DecorationTilemap.SetTile(tile + new Vector3Int(0, 1, 0), null);
					}
					
					else if (DecorationTilemap.GetTile(tile) == Lamp[1]) {
						CorruptionDecorationTilemap.SetTile(tile, CorruptedLamp[1]);
						CorruptionDecorationTilemap.SetTile(tile + new Vector3Int(0, -1, 0), CorruptedLamp[0]);
						DecorationTilemap.SetTile(tile + new Vector3Int(0, -1, 0), null);
					}
					
					else if (DecorationTilemap.GetTile(tile) == Car) CorruptionDecorationTilemap.SetTile(tile, CorruptedCar);
					else if (DecorationTilemap.GetTile(tile) == Hen) CorruptionDecorationTilemap.SetTile(tile, CorruptedHen);
					
					DecorationTilemap.SetTile(tile, null);
				}

				FloorTilemap.SetTile(tile, null);
				
				_expansionTiles.RemoveAt(i);
				_expansionTiles.Add(tile);
			}
			yield return new WaitForSeconds(0.7f);
			
			_generating = false;
		}
	}
}
