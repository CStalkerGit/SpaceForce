using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlidingPanel : MonoBehaviour
{
    public bool animated;

    public bool showing;

    public const float defaultPosX = -300;
    const float animMinSpeed = 200;
    const float animMaxSpeed = 800;

    static int stageNum;

    void Awake()
    {

    }

    void Update()
    {
        if (animated)
        {
            var pos = transform.localPosition;
            float s = Mathf.Abs(pos.x * animMaxSpeed / defaultPosX + animMinSpeed);
            s = Mathf.Clamp(s, animMinSpeed, animMaxSpeed);
            
            if (showing)
            {

                if (pos.x < 0) pos.x += Time.deltaTime * s;
                if (pos.x > 0)
                {
                    pos.x = 0;
                    animated = false;
                }
            }
            else
            {
                if (pos.x > defaultPosX) pos.x -= Time.deltaTime * s;
                if (pos.x < defaultPosX)
                {
                    pos.x = defaultPosX;
                    animated = false;
                    if (MainMenu.activePanel) MainMenu.activePanel.StartSlidingAnimation(true);
                    else if (stageNum > 0) SceneManager.LoadScene($"Level{stageNum}");
                }
            }
            transform.localPosition = pos;
        }
    }

    void StartSlidingAnimation(bool showing)
    {
        animated = true;
        this.showing = showing;
        gameObject.SetActive(true);
    }

    public void Show()
    {
        if (MainMenu.activePanel) MainMenu.activePanel.StartSlidingAnimation(false);
        else StartSlidingAnimation(true);
        MainMenu.activePanel = this;
    }

    public void StageSelect(int num)
    {
        stageNum = num;
        if (MainMenu.activePanel) MainMenu.activePanel.StartSlidingAnimation(false);
        MainMenu.activePanel = null;
    }

    public static void Show(string name)
    {
        foreach (var e in MainMenu.panels)
        {
            if (e.name != name) continue;

            e.Show();
            return;
        }

        Debug.LogWarning($"Panel {name} was not found!");
    }
}
