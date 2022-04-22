using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class ScrollingMap : MonoBehaviour
{
    private Tilemap tilemap;

    private Vector3 tilemapPos;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // скроллинг карты вниз
        tilemapPos.y -= Engine.scrollingSpeed * Time.deltaTime;
        transform.position = tilemapPos;
    }

    /// <summary>
    /// Проверка, не вышел ли объект за границы экрана
    /// </summary>
    /// <param name="position">текущая позиция объекта</param>
    /// <param name="offset">дополнительное смещение от краев экрана</param>
    public bool OutOfBounds(Vector3 position, float offset)
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        position -= tilemapPos; // смещение относительно текущего положения камеры

        if (position.x < -width / 2 - offset || position.x > width / 2 + offset) return true;
        if (position.y < -height / 2 - offset || position.y > height / 2 + offset) return true;
        return false;
    }

    public bool IsMapEnd()
    {
        if (transform.position.y < -tilemap.size.y - 8) return true;
        return false;
    }
}
