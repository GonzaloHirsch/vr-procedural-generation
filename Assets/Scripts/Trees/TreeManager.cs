using UnityEngine;

public class TreeManager : Framework.MonoBehaviorSingleton<TreeManager>
{
    [Header("Prefab")]
    public GameObject treePrefab;
    private int treeCount = 0;


    // Returns the prefab for someone to instantiate it
    public GameObject GetTreeToPlant() {
        return treePrefab;
    }

    // Used to notify about new trees
    public void TreePlanted() {
        this.treeCount++;
    }

    public void PlantTree(RaycastHit hit){
        // Only instantiate if the surface is teleportable
        if (hit.transform.gameObject.CompareTag(Constants.TELEPORT_TAG))
        {
            // Instantiate object once it hits
            GameObject obj = GameObject.Instantiate(this.treePrefab, hit.point, Quaternion.identity);
            obj.transform.up = hit.normal;
            // Make sure the parent is the manager for cleaning purposes
            obj.transform.parent = this.gameObject.transform;
            // Set a name for good meassure
            obj.gameObject.name = "Tree_" + this.treeCount;
            // Increase the tree counter
            this.treeCount++;
        }
    }
}
