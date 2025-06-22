using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelProgressChecker : MonoBehaviour
{
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private string unlockKey = "Level2Unlocked";
    [SerializeField] private string targetTag = "Ball10";

    [Header("Звук разблокировки")]
    [SerializeField] private AudioClip unlockSound;
    [SerializeField] private AudioSource audioSource;

    private bool isUnlocked = false;

    void Start()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.gameObject.SetActive(false);
            nextLevelButton.onClick.AddListener(GoToLevelSelect);
        }
    }

    void Update()
    {
        if (!isUnlocked && GameObject.FindGameObjectWithTag(targetTag) != null)
        {
            isUnlocked = true;

            if (nextLevelButton != null)
                nextLevelButton.gameObject.SetActive(true);

            if (audioSource != null && unlockSound != null)
                audioSource.PlayOneShot(unlockSound);

            PlayerPrefs.SetInt(unlockKey, 1);
            PlayerPrefs.Save();
        }
    }

    void GoToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }
}