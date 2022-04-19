using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlackBar : MonoBehaviour
{
    public bool left;

    private int lastWidth;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CheckScreenSize();
    }

    void FixedUpdate()
    {
        CheckScreenSize();
    }

    // FIXME
    private void CheckScreenSize()
    {
        int pixelWidth = Camera.main.pixelWidth;

        if (lastWidth != pixelWidth)
        {
            lastWidth = pixelWidth;

            int tiling = pixelWidth / (int)(256 / Camera.main.aspect);
            spriteRenderer.size = new Vector2(tiling * 2, 20);
            tiling += 6; // offset for map's center to edge
            transform.position = new Vector3(left ? -tiling : tiling, 0, 0);
        }
    }
}
