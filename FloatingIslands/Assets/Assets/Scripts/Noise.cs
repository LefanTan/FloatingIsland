using UnityEngine;
using System.Collections;

public static class Noise {

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, int persistance, int lacunarity) {
		float[,] noiseMap = new float[mapWidth,mapHeight];

		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MaxValue;
		float minNoiseHeight = float.MinValue;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

					float amptitude = 1;
					float frequency = 1;
					float noiseHeight = 1;

				for (int i = 0; i < octaves; i++){
					float sampleX = x / scale;
					float sampleY = y / scale;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;	
					noiseHeight += perlinValue *  amptitude;

					amptitude *= persistance;
					frequency *= lacunarity;
				}

				if(noiseHeight > maxNoiseHeight){
					maxNoiseHeight = noiseHeight;
				}else if(noiseHeight < minNoiseHeight){
					minNoiseHeight = noiseHeight;
				}
				noiseMap[x, y] = noiseHeight;
			}
		}

		//normalize noise map to value between 0 and 1
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
			}
		}

		return noiseMap;
	}

}