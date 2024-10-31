using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
    [Header("Scene Assignments")]
    public int mainMenuScene = 0;         // Assignable scene for the main menu
    public int tutorialScene = 1;         // Assignable scene for the tutorial
    public int firstStageScene = 2;       // Assignable scene for the first stage
    public int finalScene = 3;            // Assignable scene for the final stage

    [Header("Optional Main Menu Button")]
    public bool hasMainMenuButton = false; // Toggle for MainMenu button visibility

    public void PlayGame()
    {
        LoadScene(firstStageScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        // Reload the First Stage Scene to restart the game from the beginning
        LoadScene(firstStageScene);
    }

    public void RestartTutorial()
    {
        // Restart Tutorial Scene specifically
        LoadScene(tutorialScene);
    }

    public void FinalStage()
    {
        // Restart Final Scene specifically
        LoadScene(finalScene);
    }

    public void MainMenu()
    {
        if (hasMainMenuButton)
        {
            LoadScene(mainMenuScene);
        }
    }

    private void LoadScene(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(sceneIndex);
        }
        else
        {
            Debug.LogWarning("Scene index out of range or not set.");
        }
    }
}
