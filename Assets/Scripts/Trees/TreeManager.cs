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
}
