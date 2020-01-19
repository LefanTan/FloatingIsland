using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour
{
    public int islandCount;
    public Vector3 skyboxBounds;
    public int seed;
    public GameObject islandPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] islands = new GameObject[islandCount];
        Vector3 islandPos;
        for (int i = 0; i < islandCount; i++){
    
            islandPos = new Vector3(Random.Range(-skyboxBounds.x, skyboxBounds.x), Random.Range(-skyboxBounds.y, skyboxBounds.y), Random.Range(-skyboxBounds.z, skyboxBounds.z));
            Debug.Log(islandPos);
            GameObject island = GameObject.Instantiate(islandPrefab, islandPos, new Quaternion(0,0,0,0));
            island.GetComponentInChildren<MapGenerator>().GenerateMap();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
