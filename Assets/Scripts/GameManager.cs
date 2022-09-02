using UnityEngine;

public class GameManager : Framework.MonoBehaviorSingleton<GameManager>
{
    [Header("Player")]
    public GameObject playerPrefab;
    private GameObject playerInstance;
    private PlayerController playerController;
    public Vector3 playerInitialPosition = new Vector3(0, 2, 0);

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
        this.playerInstance = GameObject.Instantiate(playerPrefab, this.playerInitialPosition, Quaternion.identity);
        this.playerController = this.playerInstance.GetComponent<PlayerController>();
    }

    public void UseTeleportMode() {
        this.playerController.ChangePlayerMode(Constants.PLAYER_MODES.TELEPORT);
    }
    
    public void UseTreeMode() {
        this.playerController.ChangePlayerMode(Constants.PLAYER_MODES.TREE);
    }
}
