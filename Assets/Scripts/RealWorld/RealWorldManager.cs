using UnityEngine.SceneManagement;

public class RealWorldManager : Framework.MonoBehaviorSingleton<RealWorldManager>
{
    // Loads the game scene
    public void PlayGame() {
        SceneManager.LoadScene(Constants.GAME_SCENE, LoadSceneMode.Single);
    }
}
