using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Framework.MonoBehaviorSingleton<GameManager>
{
    [Header("Player")]
    public GameObject playerPrefab;
    private GameObject playerInstance;
    private PlayerController playerController;
    public Vector3 playerInitialPosition = new Vector3(0, 2, 0);
    [Header("Tree")]
    public int treeTarget = 20;
    public int currentTreeCount = 0;
    public int previousTreeCount = 0;
    [Header("Ending")]
    public GameObject gameOverPanel;
    private bool stayInVR = false;

    void Start()
    {
        // Set up the game over panel
        this.gameOverPanel.SetActive(false);
        this.stayInVR = false;
        // Set Up all the map
        this.SetupMap();
        // Setup player
        this.SetupPlayer();
    }

    void Update()
    {
        this.CheckTreeProgress();
    }

    private void CheckTreeProgress()
    {
        this.currentTreeCount = TreeManager.Instance.GetTreeCount();
        if (this.currentTreeCount > this.previousTreeCount) {
            MeshGenerator.Instance.UpdateMeshColor((float)this.currentTreeCount / this.treeTarget);
            RisingSun.Instance.MoveSun((float)this.currentTreeCount / this.treeTarget);
            this.previousTreeCount = this.currentTreeCount;
        }
        if (this.currentTreeCount >= this.treeTarget && !this.stayInVR && !this.gameOverPanel.activeSelf) {
            this.gameOverPanel.SetActive(true);
            this.gameOverPanel.transform.LookAt(this.playerInstance.transform.position, this.gameOverPanel.transform.up);
            Vector3 rotation = this.gameOverPanel.transform.rotation.eulerAngles;
            this.gameOverPanel.transform.rotation = Quaternion.Euler(new Vector3(0f, rotation.y, 0f));
        }
    }

    private void SetupMap()
    {
        // Create the ground
        MeshGenerator.Instance.GenerateTerrain();
        // Add the columns
        ColumnManager.Instance.GenerateColumns();
    }

    private void SetupPlayer()
    {
        this.playerInstance = GameObject.Instantiate(playerPrefab, this.playerInitialPosition, Quaternion.identity);
        this.playerController = this.playerInstance.GetComponent<PlayerController>();
    }

    public void UseTeleportMode()
    {
        this.playerController.ChangePlayerMode(Constants.PLAYER_MODES.TELEPORT);
    }

    public void UseTreeMode()
    {
        this.playerController.ChangePlayerMode(Constants.PLAYER_MODES.TREE);
    }

    public void StayInVR()
    {
        this.gameOverPanel.SetActive(false);
        this.stayInVR = true;
    }

    public void GoToRealWorld()
    {
        // Set the game as played
        GameState.Instance.isVideogamePlayed = true;
        // Load the real world scene
        SceneManager.LoadScene(Constants.REAL_SCENE, LoadSceneMode.Single);
    }
}
