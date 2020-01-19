using UnityEngine;
using System.Collections;

public static class Noise {

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, AnimationCurve xCurve, AnimationCurve yCurve, float normMultiplier) {
		float[,] noiseMap = new float[mapWidth,mapHeight];

		System.Random rng = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

		for(int i = 0; i < octaves; i++){
			float offsetX = rng.Next(-100000, 100000) + offset.x;
			float offsetY = rng.Next(-100000, 100000) + offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		if (scale <= 0) {
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
		
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / scale * frequency +octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude - GetNormalized(xCurve, yCurve, x, y, mapWidth, mapHeight) * normMultiplier;
					//noiseHeight += perlinValue * amplitude - Distance_SquareRt(x, y, mapWidth, mapHeight);

					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}
				noiseMap [x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
			}
		}

		return noiseMap;
	}

	private static float GetNormalized(AnimationCurve xCurve, AnimationCurve yCurve, int x, int y, int width, int height){

		float dx =  2f * (float)x/ (float)width - 1f;
		float dy = 2f * (float)y / (float)height -1f;

	//	Debug.Log(dx + "  " + dy);
		float xEva = xCurve.Evaluate(dx);
		float yEva = yCurve.Evaluate(dy);
		return Mathf.Sqrt(xEva * xEva + yEva * yEva);
	}

	private static float Distance_SquareRt (float x, float y, int width, int height){

		float dx =  2 * x / width - 1;
		float dy = 2 * y / height - 1;

		return Mathf.Sqrt( dx * dx + dy*dy);
	}

}