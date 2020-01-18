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

	public bool autoUpdate;
	public TerrainType[] regions;

	public void GenerateMap() {
		float[,] heightMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, noiseScale);
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
	
}

[System.Serializable]
public struct TerrainType {
	public string name;
    public float height;
    public Color color;
}