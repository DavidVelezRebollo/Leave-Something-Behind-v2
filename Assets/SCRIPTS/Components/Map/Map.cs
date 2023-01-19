using System.Collections;
using LSB.Shared;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LSB.Components.Core {
	public class Map : MonoBehaviour {
		[Header("Tilemaps")]
		[SerializeField] private Tilemap FloorTilemap;

		[Space(10)] [Header("Tiles")] 
		[SerializeField] private TileBase Grass;
		[SerializeField] private TileBase[] Decoration;
		[SerializeField] private TileBase[] Tree;
		[SerializeField] private TileBase[] Lamp;
		[SerializeField] private TileBase[] Tower;
		[SerializeField] private TileBase Car;
		[SerializeField] private TileBase Hen;

		[Space(10)] [Header("Map variables")] 
		[SerializeField] private int MapHeight = 50;
		[SerializeField] private int MapWidth = 50;
		[Range(0, 1)]
		[SerializeField] private float Scale = 0.1f;
		[Range(0, 1)]
		[SerializeField] private float XOffset = 0.5f;
		[Range(0, 1)]
		[SerializeField] private float YOffset = 0.5f;
		
	
		private const float _LAMP_PROBABILITY = 0.05f;
		private const float _TOWER_PROBABILITY = 0.10f;
		private const float _CAR_PROBABILITY = 0.15f;
		private const float _HEN_PROBABILITY = 0.20f;
		private const float _TREES_PROBABILITY = 0.30f;
		private const float _DECORATION_PROBABILITY = 0.40f;
		
		private Props[,] _cellMap;

		private void Start() {
			_cellMap = new Props[MapWidth, MapHeight];
			
			GenerateCellMap();
			DrawMap();
		}

		private void GenerateCellMap() {
			float[,] noiseMap = GeneratePerlinNoise();
			
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

		private void DrawMap() {
			for(int y = 0; y < MapHeight; y++)
				for (int x = 0; x < MapWidth; x++) {
					Vector3Int pos = new Vector3Int(MapWidth / 2 - x, MapHeight / 2 - y, 0);
					TileBase tile = null;

					switch (_cellMap[x, y]) {
						case Props.Tiled:
							continue;
						case Props.Grass:
							tile = Grass;
							break;
						case Props.Decoration:
							int index = Random.Range(0, Decoration.Length);
							
							tile = Decoration[index];
							break;
						case Props.Tree:
							if (y == 0) continue;
							if (_cellMap[x, y - 1] == Props.Tree) {
								tile = Tree[0];
								
								FloorTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Tree[1]);
								_cellMap[x, y - 1] = Props.Tiled;
							}

							break;
						case Props.Lamp:
							if (y == 0) continue;
							if (_cellMap[x, y - 1] == Props.Lamp) {
								tile = Lamp[0];
								
								FloorTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Lamp[1]);
								_cellMap[x, y - 1] = Props.Tiled;
							}
							
							break;
						case Props.Tower:
							if (y == 0) continue;
							if (_cellMap[x, y - 1] == Props.Tower) {
								tile = Tower[0];
								
								FloorTilemap.SetTile(pos + new Vector3Int(0, 1, 0), Tower[1]);
								_cellMap[x, y - 1] = Props.Tiled;
							}
							
							break;
						case Props.Car:
							tile = Car;
							break;
						case Props.Hen:
							tile = Hen;
							break;
					}
					
					FloorTilemap.SetTile(pos, tile);
				}
		}

		private float[,] GeneratePerlinNoise() {
			float[,] noiseMap = new float[MapWidth, MapHeight];

			for(int x = 0; x < MapWidth; x++)
				for (int y = 0; y < MapHeight; y++) {
					float noise = Mathf.PerlinNoise(x * Scale + XOffset, y * Scale + YOffset);

					noiseMap[x, y] = noise;
				}

			return noiseMap;
		}
	}
}