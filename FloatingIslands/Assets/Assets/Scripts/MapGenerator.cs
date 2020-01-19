using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {
		Noise,
		Color
	}
	public DrawMode drawMode;
	public int mapWidth;
	public int mapHeight;
	public float noiseScale;
	public int octaves;
	[Range(0,1)]
	public float persistance; //affects amptitude so that amptitude decreases with octaves
	public float lacunarity; //affects frequency so that frequency increases with octaves 
	public int seed;
	public Vector2 offset;

	public bool autoUpdate;
	public TerrainType[] regions;

	public void GenerateMap() {
		float[,] heightMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
		Color[] colorMap = new Color[mapWidth * mapHeight];

		for (int y = 0; y < mapHeight; y++){
			for (int x = 0; x < mapWidth; x ++){
				float currentHeight = heightMap[x, y];
				for (int i = 0; i < regions.Length; i ++) {
					if (currentHeight < regions[i].height){
						colorMap[y * mapWidth + x] = regions[i].color;
						break;
					}
				}
			}
		}

		if (drawMode == DrawMode.Noise){
			MapDisplay display = FindObjectOfType<MapDisplay> ();
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
		} else if (drawMode == DrawMode.Color){
			MapDisplay display = FindObjectOfType<MapDisplay>();
			display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
		}

		
	}
		
	void OnValidate(){
		if(mapWidth < 1){
			mapWidth = 1;
		}
		if(mapHeight < 1){
			mapHeight = 1;
		}
		if(octaves < 0){
			octaves = 0;
		}
		if(lacunarity < 1 ){
			lacunarity = 1;
		}
	}

	void Start() {
		GenerateMap();
	}

}

[System.Serializable]
public struct TerrainType {
	public string name;
    public float height;
    public Color color;
}