using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public SceneData sceneData; // Reference to the ScriptableObject
    
    // Called when the "Next level" button is clicked
    public void NextLevel()
    {
        // Get the next scene index from sceneData
        int nextSceneIndex = SceneUtility.GetBuildIndexByScenePath(sceneData.previousScene) + 1;
        
        // Check if the next scene index is valid
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene if it exists
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If no next scene exists, load the EndGame scene
            SceneManager.LoadScene("EndGame");
        }
    }

    // Called when the "Quit" button is clicked
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
