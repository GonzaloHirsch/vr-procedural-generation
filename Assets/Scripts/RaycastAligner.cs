using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAligner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Raycast Distance")]
    public float raycastDistance = 100f;


    // Start is called before the first frame update
    void Start()
    {
        PositionRaycast();
    }

    void PositionRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            GameObject clone = Instantiate(prefab, hit.point, spawnRotation);
        }
    }
}
