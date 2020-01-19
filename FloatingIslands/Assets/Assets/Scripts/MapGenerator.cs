﻿using UnityEngine;
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
	public float normMultiplier;
	public Vector2 offset;
	public AnimationCurve xNoiseCurve;
	public AnimationCurve yNoiseCurve;

	[Range(0,3)]
    public int levelOfDetail;
	const int mapChunkSize = 121;


	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate;
	public TerrainType[] regions;

	public MeshGenerator meshGenerator = new MeshGenerator();

	public void GenerateMap() {
		Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
		float[,] heightMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset, xNoiseCurve, yNoiseCurve, normMultiplier);

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
			MapDisplay display = this.gameObject.GetComponent<MapDisplay>();
			display.DrawMesh(meshGenerator.GenerateTerrainMesh(heightMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
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