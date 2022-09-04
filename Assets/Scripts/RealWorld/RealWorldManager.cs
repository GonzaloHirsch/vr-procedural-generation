using UnityEngine.SceneManagement;
using UnityEngine;

public class RealWorldManager : Framework.MonoBehaviorSingleton<RealWorldManager>
{
    public GameObject secretDoor;

    void Start() {
        this.secretDoor.SetActive(GameState.Instance.isSecretFound);
    }

    public void PlayGame()
    {
        // Set the game as played, that way when the user is back it's ready
        GameState.Instance.isVideogamePlayed = true;
        // Load the videogame scene
        SceneManager.LoadScene(Constants.GAME_SCENE, LoadSceneMode.Single);
    }

    public void PrepareSecretEnding()
    {
        // Only allow the player to get the secret ending after playing the videogame
        if (GameState.Instance.isVideogamePlayed)
        {
            GameState.Instance.isSecretFound = true;
            // Prepares the secret ending
            this.secretDoor.SetActive(true);
        }
    }
}
