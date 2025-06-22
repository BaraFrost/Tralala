using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallUnlockChecker : MonoBehaviour
{
    [System.Serializable]
    public class BallButton
    {
        public string ballTag;
        public Image iconImage;
        public Sprite unlockedSprite;
        public Button button;
        public MusicOnClick musicScript;
    }

    [SerializeField] private List<BallButton> ballButtons;

    void Start()
    {

    }
    private void Update()
    {
        foreach (var bb in ballButtons)
        {
            bool isUnlocked = PlayerPrefs.GetInt($"BallUnlocked_{bb.ballTag}", 0) == 1;

            if (isUnlocked)
            {
                if (bb.iconImage != null && bb.unlockedSprite != null)
                    bb.iconImage.sprite = bb.unlockedSprite;

                if (bb.button != null)
                    bb.button.interactable = true;

                if (bb.musicScript != null)
                    bb.musicScript.enabled = true;
            }
            else
            {
                // Не изменяем иконку, не делаем её прозрачной — просто отключаем нажатие
                if (bb.button != null)
                    bb.button.interactable = false;

                if (bb.musicScript != null)
                    bb.musicScript.enabled = false;
            }
        }
    }
}