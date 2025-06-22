using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using YG;
using System.Collections.Generic;

public class BallImpulseShaker : MonoBehaviour
{
    private bool canClick = true;
    private float clickCooldown = 0.5f;

    [Header("Impulse Settings")]
    [SerializeField] private float minImpulse = 1f;
    [SerializeField] private float maxImpulse = 5f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI usesText;
    [SerializeField] private GameObject noUsesImage;
    [SerializeField] private Button bombButton;

    [Header("Object to Disable on Use")]
    [SerializeField] private GameObject objectToDisable;
    [SerializeField] private float disableTime = 10f;

    [Header("Ad Settings")]
    public string rewardID;

    private const string UsesPrefsKey = "BallImpulseUses";
    private int availableUses;

    private void Start()
    {
        if (!PlayerPrefs.HasKey(UsesPrefsKey))
        {
            availableUses = 1;
            PlayerPrefs.SetInt(UsesPrefsKey, availableUses);
        }
        else
        {
            availableUses = PlayerPrefs.GetInt(UsesPrefsKey);
        }

        UpdateUI();
    }

    public void OnBombButtonClick()
    {
        if (!canClick) return;

        // ✅ Отправка единственной метрики — клик по кнопке
        YG2.MetricaSend("bomb_button_click");

        if (availableUses > 0)
        {
            StartCoroutine(HandleButtonCooldown());

            ApplyImpulse();
            availableUses--;
            PlayerPrefs.SetInt(UsesPrefsKey, availableUses);
            UpdateUI();
        }
        else
        {
            YG2.RewardedAdvShow(rewardID, () =>
            {
                availableUses += 3;
                PlayerPrefs.SetInt(UsesPrefsKey, availableUses);
                UpdateUI();

                StartCoroutine(HandleButtonCooldown());
            });
        }
    }

    private void ApplyImpulse()
    {
        BallController[] balls = FindObjectsOfType<BallController>();
        foreach (var ball in balls)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb == null) continue;

            float angle = Random.Range(60f, 120f) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            float impulse = Random.Range(minImpulse, maxImpulse);
            rb.AddForce(dir * impulse, ForceMode2D.Impulse);
        }

        if (CameraShake.Instance != null)
            CameraShake.Instance.ShakeCamera();

        if (objectToDisable != null)
            StartCoroutine(DisableTemporarily(objectToDisable, disableTime));
    }

    private IEnumerator DisableTemporarily(GameObject target, float duration)
    {
        target.SetActive(false);
        yield return new WaitForSeconds(duration);
        target.SetActive(true);
    }

    private void UpdateUI()
    {
        bool hasUses = availableUses > 0;

        if (usesText != null)
        {
            usesText.gameObject.SetActive(hasUses);
            usesText.text = availableUses.ToString();
        }

        if (noUsesImage != null)
            noUsesImage.SetActive(!hasUses);

        if (bombButton != null)
            bombButton.interactable = true;
    }

    private IEnumerator HandleButtonCooldown()
    {
        canClick = false;

        if (usesText != null)
            usesText.gameObject.SetActive(false);

        if (bombButton != null)
            bombButton.interactable = false;

        yield return new WaitForSeconds(clickCooldown);

        canClick = true;
        UpdateUI();
    }
}
