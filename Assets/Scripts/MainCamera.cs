using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {

        SceneManager.LoadSceneAsync(1);
    }

    public void Introduction()
    {
        SceneManager.LoadSceneAsync(4);
    }
}