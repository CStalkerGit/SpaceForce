using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image[] imgHP;

    static HUD instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public static void UpdateHealthBar(int countHP)
    {
        if (!instance) return;

        for (int i = 0; i < instance.imgHP.Length; i++)
        {
            instance.imgHP[i].gameObject.SetActive(countHP < i + 1 ? false : true);
        }
    }
}
