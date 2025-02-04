using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public SceneData sceneData; // Reference to the ScriptableObject
    
    // Called when the "Restart" button is clicked
    public void RestartLevel()
    {
        // Reload the previous scene
        SceneManager.LoadScene(sceneData.previousScene);
    }

    // Called when the "Quit" button is clicked
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
