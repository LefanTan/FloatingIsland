using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public void Start(){
		GenerateMap();
	}

	public enum DrawMode {
		Noise,
		Color,
		Mesh
	}
	public DrawMode drawMode;
	public float noiseScale;
	public int octaves;
	[Range(0,1)]
	public float persistance; //affects amptitude so that amptitude decreases with octaves
	public float lacunarity; //affects frequency so that frequency increases with octaves 
	public int seed;
	public Vector2 offset;

	[Range(0,6)]
    public int levelOfDetail;
	const int mapChunkSize = 241;


	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate;
	public TerrainType[] regions;

	public void GenerateMap() {
		float[,] heightMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
		Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

		for (int y = 0; y < mapChunkSize; y++){
			for (int x = 0; x < mapChunkSize; x ++){
				float currentHeight = heightMap[x, y];
				for (int i = 0; i < regions.Length; i ++) {
					if (currentHeight < regions[i].height){
						colorMap[y * mapChunkSize + x] = regions[i].color;
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
			display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.Mesh){
			MapDisplay display = FindObjectOfType<MapDisplay>();
			display.DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
		}

		
	}
	
	void OnValidate(){
		if(octaves < 0){
			octaves = 0;
		}
		if(lacunarity < 1 ){
			lacunarity = 1;
		}
	}

}

[System.Serializable]
public struct TerrainType {
	public string name;
    public float height;
    public Color color;
}