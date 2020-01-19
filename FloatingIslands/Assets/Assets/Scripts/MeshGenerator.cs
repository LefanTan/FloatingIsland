using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{

    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int meshSimplificationIncrement = (levelOfDetail > 0) ? levelOfDetail * 2 : 1;
        int verticesPerLine = (width - 1)/ meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(2 * verticesPerLine, 2 * verticesPerLine);
        int vertexIndex = 0;

        // Create top
        for (int y = 0; y < height; y += meshSimplificationIncrement){
            for (int x = 0; x < width; x += meshSimplificationIncrement){
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x,y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float) width, y / (float) height);

                vertexIndex ++;
            }
        }
        vertexIndex = 0;
        for (int y = 0; y < height; y += meshSimplificationIncrement){
            for (int x = 0; x < width; x += meshSimplificationIncrement){
                if (x < width -1 && y < height - 1){
                    if (meshData.vertices[vertexIndex].y > 0f || meshData.vertices[vertexIndex + verticesPerLine + 1].y > 0f || meshData.vertices[vertexIndex + verticesPerLine].y > 0f) {
                        meshData.AddTrianlge(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    }
                    if (meshData.vertices[vertexIndex].y > 0f || meshData.vertices[vertexIndex + verticesPerLine + 1].y > 0f || meshData.vertices[vertexIndex + 1].y > 0f) {
                        Debug.Log("2");
                        meshData.AddTrianlge(vertexIndex,vertexIndex + 1, vertexIndex + verticesPerLine + 1);
                    }
                }
                vertexIndex ++;
            }
        }

        // // Mirror to Bottom
        // for (int y = 0; y < height; y += meshSimplificationIncrement){
        //     for (int x = 0; x < width; x += meshSimplificationIncrement){
        //         meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, -heightCurve.Evaluate(heightMap[x,y]) * heightMultiplier - 0.001f, topLeftZ - y);
        //         meshData.uvs[vertexIndex] = new Vector3(x / (float) width, y / (float) height);

        //         if (x > 0 && y > 0){
        //             meshData.AddTrianlge(vertexIndex, vertexIndex + verticesPerLine, vertexIndex + verticesPerLine + 1);
        //             meshData.AddTrianlge(vertexIndex + verticesPerLine + 1,vertexIndex + 1, vertexIndex);
        //         }
        //         vertexIndex ++;
        //     }
        // }

        return meshData;
    }
}


public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight){
        vertices = new Vector3[2 * meshWidth * meshHeight];
        uvs = new Vector2[2 * meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6 * 2];
    }

    public void AddTrianlge(int a, int b, int c){
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh(){
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
