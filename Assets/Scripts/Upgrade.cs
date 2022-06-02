using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation
{
    Top,
    Side,
    Bottom,
    Option
}

public class Upgrade : MonoBehaviour
{
    public Orientation orientation;
    [Tooltip("Смещение положения поверапа относительно центра игрока, в пикселях")]
    public Vector2Int offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddToParent(Transform parent, Upgrade upgradePrefab)
    {
        Upgrade sideUpgrade = null;

        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i).GetComponent<Upgrade>();
            if (!child) continue;
            if (child.orientation == upgradePrefab.orientation)
            {
                if (child.orientation == Orientation.Side)
                {
                    // если слева уже стоит поверап, снимаем поверап справа
                    if (sideUpgrade) Destroy(child.gameObject); else sideUpgrade = child;
                    continue;
                }
            }
        }
        var upgrade = Instantiate(upgradePrefab, parent);

        Vector3 position = new Vector3(
            upgradePrefab.offset.x / Engine.PPU,
            upgradePrefab.offset.y / Engine.PPU,
            0);
        if (sideUpgrade) position.x = -position.x; // flip side
        upgrade.transform.localPosition = position;
    }
}
