using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void LoadLevelSelectScene()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level2");
    }
}
