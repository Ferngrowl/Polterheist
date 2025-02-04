using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelectMenu; // Reference to the Level Select Submenu

    public void StartGame()
    {
        // Load the first level
        SceneManager.LoadScene("LevelOne"); 
    }

    public void ToggleLevelSelectMenu()
    {
        // Toggle the visibility of the level select submenu
        levelSelectMenu.SetActive(!levelSelectMenu.activeSelf);
    }
    
    public void LoadLevel(string levelName)
    {
        // Load the selected level
        SceneManager.LoadScene(levelName);
    }
}
