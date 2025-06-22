using UnityEngine;

public class SaveOnExit : MonoBehaviour
{
    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.Save();
        }
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            PlayerPrefs.Save();
        }
    }
}
