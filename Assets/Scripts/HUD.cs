using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image hpBar;
    static HUD instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public static void UpdateHealthBar(int countHP)
    {
        float scale = countHP / 100.0f;
        instance.hpBar.transform.localScale = new Vector3(scale, 1, 1);
    }
}
