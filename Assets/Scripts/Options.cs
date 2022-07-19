using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    private static Options instance;

    void Awake()
    {
        instance = this;
        Hide();
    }

    public static void Show()
    {
        Engine.IsPaused = true;
        instance.transform.localPosition = new Vector3(0, 0, 0);
        Time.timeScale = 0;
    }

    public static void Hide()
    {
        Engine.IsPaused = false;
        instance.transform.localPosition = new Vector3(0, -500, 0);
        Time.timeScale = 1;
    }

    public void HideOptions()
    {
        Hide();
    }

    public void SoundVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void MusicVolume(float volume)
    {
        Music.GlobalVolume(volume);
    }
}
