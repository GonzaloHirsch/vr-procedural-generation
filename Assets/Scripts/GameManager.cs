using UnityEngine;

public class GameManager : Framework.MonoBehaviorSingleton<GameManager>
{
    [Header("Player")]
    public GameObject playerPrefab;
    public Vector3 playerInitialPosition = Vector3.zero;

    void Start() {
        // Set Up all the map
        this.SetupMap();
        // Setup player
        this.SetupPlayer();
    }    

    private void SetupMap() {
        // Create the ground
        MeshGenerator.Instance.GenerateTerrain();
        // Add the columns
        ColumnManager.Instance.GenerateColumns();
    }

    private void SetupPlayer() {
        GameObject.Instantiate(playerPrefab, this.playerInitialPosition, Quaternion.identity);
    }
}
