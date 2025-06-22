using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void GoToFunScene()
    {
        BallStateManager.Instance.SaveState();
        SceneManager.LoadScene("Fun");
    }

    public void ReturnToLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void RestartLevel()
    {
        BallStateManager.Instance.ResetState();
        SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
