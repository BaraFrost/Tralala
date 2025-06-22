using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private Button level2Button;
    [SerializeField] private string level2SceneName = "Level2";

    [SerializeField] private Button level3Button;
    [SerializeField] private string level3SceneName = "Level3";
    void Start()
    {
        if (PlayerPrefs.GetInt("Level2Unlocked", 0) == 1)
        {
            level2Button.interactable = true;
            level2Button.onClick.AddListener(() => SceneManager.LoadScene(level2SceneName));
        }
        else
        {
            level2Button.interactable = false;
        }
        if (PlayerPrefs.GetInt("Level3Unlocked", 0) == 1)
        {
            level3Button.interactable = true;
            level3Button.onClick.AddListener(() => SceneManager.LoadScene(level3SceneName));
        }
        else
        {
            level3Button.interactable = false;
        }
    }
}
