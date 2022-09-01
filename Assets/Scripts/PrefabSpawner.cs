using UnityEngine;
using System.Collections;

public class PrefabSpawner : MonoBehaviour
{

    [Header("Item to Spread")]
    public GameObject prefab;

    [Header("Quantity")]
    public int quantity;

    [Header("Size")]
    public int xSize = 20;
    public int ySize = 0;
    public int zSize = 20;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < quantity; i++)
        {
            SpawnPrefab();
        }

        //PositionRaycast();
    }

    void SpawnPrefab()
    {
        Vector3 randomPos = new Vector3(Random.Range(-xSize, xSize), Random.Range(-ySize, ySize), Random.Range(-zSize, zSize)) + transform.position;
        Instantiate(prefab, randomPos, Quaternion.identity);
    }

}
