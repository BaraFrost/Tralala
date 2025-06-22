using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GoalOverlayUI : MonoBehaviour
{
    public GameObject overlayPanel; // Panel с фоном, текстом и изображением
    public float displayTime = 2f;

    private bool isWaitingForInput = false;

    void Start()
    {
        ShowOverlay();
    }

    public void ShowOverlay()
    {
        overlayPanel.SetActive(true);
        StartCoroutine(WaitToHide());
    }

    IEnumerator WaitToHide()
    {
        isWaitingForInput = true;
        float timer = 0f;

        while (timer < displayTime && !Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.touchCount.Equals(1))
        {
            timer += Time.deltaTime;
            yield return null;
        }

        HideOverlay();
    }

    public void HideOverlay()
    {
        overlayPanel.SetActive(false);
        isWaitingForInput = false;
    }
}