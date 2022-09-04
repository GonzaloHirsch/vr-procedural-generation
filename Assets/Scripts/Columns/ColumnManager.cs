using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnManager : Framework.MonoBehaviorSingleton<ColumnManager>
{
    [Header("Item to Spread")]
    public GameObject[] prefabs;

    [Header("Quantity")]
    public int quantity;

    [Header("Size")]
    public int xSize = 20;
    public int zSize = 20;
    [Header("Raycast")]
    public float raycastDistance = 100f;
    [Header("Separation")]
    public float deltaToPlayer = 5f;

    public void GenerateColumns()
    {
        foreach (GameObject prefab in this.prefabs)
        {

            // Store the position where the player will be
            Vector3 _playerPosition = GameManager.Instance.playerInitialPosition, origin;
            Vector2 playerPosition = -new Vector2(_playerPosition.x, _playerPosition.z);
            RaycastHit hit;
            GameObject obj;
            int i = 0;
            while (i < quantity)
            {
                // Compute origin of the ray
                origin = new Vector3(
                    Random.Range(this.transform.position.x - this.xSize / 2, this.transform.position.x + this.xSize / 2),
                    this.transform.position.y,
                    Random.Range(this.transform.position.z - this.zSize / 2, this.transform.position.z + this.zSize / 2)
                );
                // Check first if the intended origin is far enough from the player
                // Cannot include Y in the comparison because it's not in the same plane
                if (Vector2.Distance(playerPosition, new Vector2(origin.x, origin.z)) > this.deltaToPlayer)
                {
                    // Launch raycast to the ground
                    if (Physics.Raycast(origin, Vector3.down, out hit, this.raycastDistance))
                    {
                        // Only instantiate if the surface is teleportable
                        if (hit.transform.gameObject.CompareTag(Constants.TELEPORT_TAG))
                        {
                            // Instantiate object once it hits
                            obj = GameObject.Instantiate(prefab, hit.point, Quaternion.identity);
                            obj.transform.up = hit.normal;
                            obj.transform.RotateAround(obj.transform.position, obj.transform.up, Random.Range(0, 360f));
                            // Make sure the parent is the manager for cleaning purposes
                            obj.transform.parent = this.gameObject.transform;
                            // Set a name for good meassure
                            obj.gameObject.name = "Column_" + i;
                            // Increase the column counter
                            i++;
                        }
                    }
                }
            }
        }
    }
}
