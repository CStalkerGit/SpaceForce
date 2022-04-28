using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class GameMap : MonoBehaviour
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
        // вертикальный скроллинг карты вниз
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

        if (position.x < -width / 2 - offset || position.x > width / 2 + offset) return true;
        if (position.y < -height / 2 - offset || position.y > height / 2 + offset) return true;
        return false;
    }

    public bool IsMapEnd()
    {
        if (transform.position.y < -tilemap.size.y - 8) return true;
        return false;
    }

    /// <summary>
    /// Получить координаты тайла в указанной позиции
    /// </summary>
    public Vector3Int GetPosition(Vector3 coord)
    {
        coord -= tilemapPos; // сдвигаем позицию тестируемого объекта относительно положения карты
        return new Vector3Int(Mathf.RoundToInt(coord.x - 0.5f), Mathf.RoundToInt(coord.y - 0.5f), 0);
    }

    //
    public void SetTile(TileBase tile, Vector3 coord)
    {
        tilemap.SetTile(GetPosition(coord), tile);
    }

    /// <summary>
    /// Возвращает Entity, связанный cо зданием на карте
    /// </summary>
    /// <param name="coord">Координаты, где должно находится здание</param>
    /// <returns>Entity здания либо null если его нет в указанной точке</returns>
    public Entity GetTileObject(Vector3 coord)
    {
        GameObject obj = tilemap.GetInstantiatedObject(GetPosition(coord));
        if (obj) return obj.GetComponent<Entity>();
        return null;
    }
}
