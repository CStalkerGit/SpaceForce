using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static SlidingPanel[] panels;
    // активная панель, которая может быть еще недоступна, пока проигрывается анимация предыдущей
    public static SlidingPanel activePanel;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        activePanel = null;

        panels = FindObjectsOfType<SlidingPanel>();
        foreach (var e in panels)
        {
            e.transform.localPosition = new Vector3(SlidingPanel.defaultPosX, e.transform.localPosition.y, 0);
            e.animated = false;
            e.gameObject.SetActive(false);
        }

        SlidingPanel.Show("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void MusicVolume(float volume)
    {
        Music.GlobalVolume(volume);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
